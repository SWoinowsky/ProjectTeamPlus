using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.Helpers;
using SteamProject.Models.DTO;
using SteamProject.Services;
using SteamProject.ViewModels;

namespace SteamProject.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly ISteamService _steamService;
    private readonly IUserGameInfoRepository _userGameInfoRepository;
    private readonly IFriendRepository _friendRepository;
    private readonly IInboxService _inboxService;

    public HomeController(UserManager<IdentityUser> userManager, IUserRepository userRepository, IGameRepository gameRepository, IUserGameInfoRepository userGameInfoRepository, ISteamService steamService, IFriendRepository friendRepository, IInboxService inboxService)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _steamService = steamService;
        _userGameInfoRepository = userGameInfoRepository;
        _friendRepository = friendRepository;
        _inboxService = inboxService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Dashboard()
    {
        string? id = _userManager.GetUserId(User);

        if (id is null)
        {
            return View();
        }
        else
        {
            UserDashboardVM dashboardVm = new UserDashboardVM();
            dashboardVm.RecentGames = new List<List<Game>>();
            dashboardVm.FollowedGames = new List<List<Game>>();
            User user = _userRepository.GetUser(id);

            dashboardVm.User = user;

            if (user.SteamId != null)
            {
                //get list of userinfo ordered by last played
                List<UserGameInfo> currentUserInfo = _userGameInfoRepository.GetAllUserGameInfo(user.Id)
                    .OrderByDescending(u => u.LastPlayed).ToList();

                //get games list for user
                List<Game>? games = _gameRepository.GetGamesListByUserInfo(currentUserInfo).Take(6).ToList();

                HashSet<Game> followedGames =
                    _gameRepository.GetGamesListByUserInfo(currentUserInfo.Where(u => u.Followed).ToList());

                // Add games and followed games to the dashboardVm
                for (var i = 0; i < games.Count; i += 3)
                {
                    List<Game> threeGames = games.Skip(i).Take(3).ToList();
                    dashboardVm.RecentGames.Add(threeGames);
                }

                for (var i = 0; i < followedGames.Count; i += 3)
                {
                    List<Game> threeGames = followedGames.Skip(i).Take(3).ToList();
                    dashboardVm.FollowedGames.Add(threeGames);
                }

                dashboardVm.BadgeImagesBase64 = new Dictionary<int, string>();

                foreach (var userBadge in user.UserBadges)
                {
                    var badgeImageBase64 = Convert.ToBase64String(userBadge.Badge.Image);
                    dashboardVm.BadgeImagesBase64[userBadge.BadgeId] = badgeImageBase64;
                }
                return View(dashboardVm);
            }



            return View();
        }
    }


    public IActionResult ShowMoreNews(int appId)
    {
        string? id = _userManager.GetUserId(User);

        if (id != null)
        {
            User user = _userRepository.GetUser(id);

            Game game = _gameRepository.GetGameByAppId(appId);

            if (game != null)
            {
                GameNewsVM gameNewsVM = _steamService.GetGameNews(game);
                gameNewsVM._game = game;
                return View(gameNewsVM);

            }
            return View();

        }
        else
        {
            return View();
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public IActionResult Friends()
    {
        string? id = _userManager.GetUserId(User);
        if (id is null)
        {
            return View();
        }
        User user = _userRepository.GetUser(id);

        if (!user.Friends.Any())
        {
            var Friends = _friendRepository.GetFriends(user.Id);
            if (Friends.Count() == 0 && user.SteamId != null)
            {
                Friends = _steamService.GetFriendsList(user.SteamId, user.Id);
                foreach (var newFriend in Friends)
                {
                    _friendRepository.AddOrUpdate(newFriend);
                }
            }
        }

        List<Friend> friends = _friendRepository.GetFriends(user.Id);
        var steamIds = _userRepository.GetAllUsers().Select(u => u.SteamId);

        foreach (var friend in friends)
        {
            if (steamIds.Contains(friend.SteamId)) {
                friend.Linked = true;
            }
        }

        FriendsPageVM vm = new(friends, user.Id, user.SteamId);
        return View(vm);
    }
}
