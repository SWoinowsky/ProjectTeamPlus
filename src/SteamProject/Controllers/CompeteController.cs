
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.Models.DTO;
using SteamProject.Services;

namespace SteamProject.Controllers;

public class CompeteController : Controller
{
    private readonly ILogger<FriendController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IFriendRepository _friendRepository;
    private readonly IGameAchievementRepository _gameAchievementRepository;
    private readonly ISteamService _steamService;

    public CompeteController(
        ILogger<FriendController> logger
        ,ISteamService steamService
        ,IUserRepository userRepository
        ,IFriendRepository friendRepository
        ,IGameAchievementRepository gameAchievementRepository
        ,IGameRepository gameRepository
        ,UserManager<IdentityUser> userManager
        )
    {
        _logger = logger;
        _steamService = steamService;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _friendRepository = friendRepository;
        _gameAchievementRepository = gameAchievementRepository;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Index( string friendSteamId, int appId )
    {
        var gameIdFound = _gameRepository.GetGameByAppId(appId).Id;
        
        var id = _userManager.GetUserId( User );
        var mySteamId = _userRepository.GetUser( id ).SteamId;

        var rawAchievements = new List<SchemaAchievement>();
        rawAchievements = _steamService.GetSchema( appId ).game.availableGameStats.achievements.ToList<SchemaAchievement>();
        
        var gameAchievements = new List<GameAchievement>();
        foreach( var rawAch in rawAchievements )
        {
            var addMe = new GameAchievement( rawAch );
            addMe.GameId = gameIdFound;
            gameAchievements.Add(addMe);
            _gameAchievementRepository.AddOrUpdate(addMe);
        }

        // var myRawAchievementData = new AchievementRoot();
        // myRawAchievementData = _steamService.GetAchievements( mySteamId, appId );

        // var myAchievements = new List<UserAchievement>();
        // foreach( var gAch in gameAchievements )
        // {
        //     foreach( var myAch in myRawAchievementData.playerstats.achievements )
        //     {
                
        //     }
        // }

        


        return View();
    }
}
