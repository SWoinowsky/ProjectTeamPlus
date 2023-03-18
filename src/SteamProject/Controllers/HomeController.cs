using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.Models.DTO;
using SteamProject.Services;
using SteamProject.ViewModels;

namespace SteamProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly ISteamService _steamService;
    private readonly IUserGameInfoRepository _userGameInfoRepository;
    private readonly IFriendRepository _friendRepository;


    public HomeController(ILogger<HomeController> logger,UserManager<IdentityUser> userManager, IUserRepository userRepository, IGameRepository gameRepository, IUserGameInfoRepository userGameInfoRepository, ISteamService steamService, IFriendRepository friendRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _steamService = steamService;
        _userGameInfoRepository = userGameInfoRepository;
        _friendRepository = friendRepository;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [Authorize]
    public IActionResult Dashboard()
    {

        string? id = _userManager.GetUserId(User);

        if (id is null)
        {
            return View();
        }
        else
        {
            UserDashboardVM dashboardVm = new UserDashboardVM();
            dashboardVm.recentGames = new List<List<Game>>();
            dashboardVm.followedGames = new List<List<Game>>();
            User user = _userRepository.GetUser(id);
            dashboardVm._user = user;

            if (user.SteamId != null)
            {
                //get list of userinfo ordered by last played
                List<UserGameInfo> currentUserInfo = _userGameInfoRepository.GetAllUserGameInfo(user.Id).OrderByDescending(u => u.LastPlayed).ToList();

                //get games list for user
                List<Game>? games = _gameRepository.GetGamesListByUserInfo(currentUserInfo).Take(12).ToList();

                List<Game>? followedGames = _gameRepository.GetGamesListByUserInfo(currentUserInfo.Where(u => u.Followed).ToList());


                //Call steam service here to get game news and add it to viewmodel for 12 most recently played games

                if (games.Any())
                {

                    for (var i = 0; i < games.Count; i+=3)
                    {
                        var threeGames = games.Skip(i).Take(3).ToList();
                        dashboardVm.recentGames.Add(threeGames);

                    }

                    for (var i = 0; i < followedGames.Count; i += 3)
                    {
                        var threeGames = followedGames.Skip(i).Take(3).ToList();
                        dashboardVm.followedGames.Add(threeGames);
                    }

                    return View(dashboardVm);
                }
            }
        }

        return View();
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
    public IActionResult Friends()
    {
        string? id = _userManager.GetUserId(User);
        if (id is null)
        {
            return View();
        }
        User user = _userRepository.GetUser(id);
        List<Friend> friends = _friendRepository.GetFriends(user.Id);
        FriendsPageVM vm = new(friends, user.Id, user.SteamId);
        return View(vm);
    }
}
