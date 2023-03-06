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


    public HomeController(ILogger<HomeController> logger,UserManager<IdentityUser> userManager, IUserRepository userRepository, IGameRepository gameRepository, IUserGameInfoRepository userGameInfoRepository, ISteamService steamService)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _steamService = steamService;
        _userGameInfoRepository = userGameInfoRepository;
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
            dashboardVm.gameTuples = new List<Tuple<Game, Game, Game>>();
            User user = _userRepository.GetUser(id);
            dashboardVm._user = user;

            if (user.SteamId != null)
            {
                //get list of userinfo ordered by last played
                List<UserGameInfo> currentUserInfo = _userGameInfoRepository.GetAllUserGameInfo(user.Id).OrderByDescending(u => u.LastPlayed).ToList();

                //get games list for user
                List<Game>? games = _gameRepository.GetGamesListByUserInfo(currentUserInfo).Take(12).ToList();

                dashboardVm.followedGames = _gameRepository.GetGamesListByUserInfo(currentUserInfo.Where(u => u.Followed).ToList());

                //Call steam service here to get game news and add it to viewmodel for 12 most recently played games

                if (games.Any())
                {

                    dashboardVm._games = games;
                    for (var i = 0; i < games.Count; i+=3)
                    {
                        var var3 = games.GetRange(i,3).ToList();

                        var game = games[i];
                        var newTuple = new Tuple<Game, Game, Game>(var3[0], var3[1], var3[2]);
                        dashboardVm.gameTuples.Add(newTuple);
                    }

                    return View(dashboardVm);
                }
            }
        }

        return View();
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
}
