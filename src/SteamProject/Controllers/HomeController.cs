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
using SteamProject.DAL.Concrete;

namespace SteamProject.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IUserGameInfoRepository _userGameInfoRepository;
    private readonly ISteamService _steamService;
    private readonly IFriendRepository _friendRepository;
    private readonly IInboxService _inboxService;
    private readonly ICompetitionRepository _competitionRepository;

    public HomeController(
        UserManager<IdentityUser> userManager,
        IUserRepository userRepository,
        IGameRepository gameRepository,
        IUserGameInfoRepository userGameInfoRepository,
        ISteamService steamService,
        IFriendRepository friendRepository,
        IInboxService inboxService,
        ICompetitionRepository competitionRepository
    )
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _steamService = steamService;
        _userGameInfoRepository = userGameInfoRepository;
        _friendRepository = friendRepository;
        _inboxService = inboxService;
        _competitionRepository = competitionRepository;
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

                // Add games and followed games to the dashboardVm using helper function
                dashboardVm.RecentGames = HelperMethods.SplitListIntoChunks(games, 3);
                dashboardVm.FollowedGames = HelperMethods.SplitListIntoChunks(followedGames.ToList(), 3);

                // Get the user's competitions
                dashboardVm.CurrentCompetitions = _competitionRepository.GetCurrentCompetitionsBySteamId(user.SteamId);

                List<Competition> previousCompetitions = _competitionRepository.GetPreviousCompetitionsBySteamId(user.SteamId);
                dashboardVm.PreviousCompetitions = HelperMethods.SplitListIntoChunks(previousCompetitions, 3);
            }

            return View(dashboardVm);
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
