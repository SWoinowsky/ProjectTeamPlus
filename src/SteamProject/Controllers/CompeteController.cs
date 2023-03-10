
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.Models.DTO;
using SteamProject.Services;
using SteamProject.ViewModels;

namespace SteamProject.Controllers;

public class CompeteController : Controller
{
    private readonly ILogger<FriendController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IUserGameInfoRepository _userGameInfoRepository;
    private readonly IFriendRepository _friendRepository;
    private readonly IGameAchievementRepository _gameAchievementRepository;
    private readonly IUserAchievementRepository _userAchievementRepository;
    private readonly ISteamService _steamService;

    public CompeteController(
        ILogger<FriendController> logger
        ,ISteamService steamService
        ,IUserRepository userRepository
        ,IFriendRepository friendRepository
        ,IGameAchievementRepository gameAchievementRepository
        ,IUserAchievementRepository userAchievementRepository
        ,IGameRepository gameRepository
        ,IUserGameInfoRepository userGameInfoRepository
        ,UserManager<IdentityUser> userManager
        )
    {
        _logger = logger;
        _steamService = steamService;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _userGameInfoRepository = userGameInfoRepository;
        _friendRepository = friendRepository;
        _gameAchievementRepository = gameAchievementRepository;
        _userAchievementRepository = userAchievementRepository;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Index( string friendSteamId, int appId )
    {
        var gameIdFound = _gameRepository.GetGameByAppId(appId).Id;
        
        var id = _userManager.GetUserId( User );
        var me = _userRepository.GetUser( id );
        var mySteamId = me.SteamId;
        var myUserId = me.Id;

        var gameIsInDb = false; 
        gameIsInDb = _gameRepository.GetGameByAppId( gameIdFound ) == null;
        if( !gameIsInDb )
        {
            var gamesToAdd = new List<Game>();
            gamesToAdd = _steamService.GetGames( mySteamId, myUserId ).ToList<Game>();
            foreach( var game in gamesToAdd )
            {
                _gameRepository.AddOrUpdate( game );
            }
        }


        var gameAchievements = new List<GameAchievement>();
        gameAchievements = _gameAchievementRepository.GetAchievementsFromGameId( gameIdFound );
        if( gameAchievements.Count() == 0 )
        {
            var rawAchievements = new List<SchemaAchievement>();
            rawAchievements = _steamService.GetSchema( appId ).game.availableGameStats.achievements.ToList<SchemaAchievement>();
            
            foreach( var rawAch in rawAchievements )
            {
                var addMe = new GameAchievement( rawAch );
                addMe.GameId = gameIdFound;
                gameAchievements.Add(addMe);
                _gameAchievementRepository.AddOrUpdate(addMe);
            }
        }
        
        var myAchievements = new List<UserAchievement>();
        myAchievements = _userAchievementRepository.GetAchievementsByGameAndUserId( gameIdFound, myUserId );
        if( myAchievements.Count() == 0 )
        {
            var myRawAchievementData = new AchievementRoot();
            myRawAchievementData = _steamService.GetAchievements( mySteamId, appId );

            foreach( var gAch in gameAchievements )
            {
                foreach( var myAch in myRawAchievementData.playerstats.achievements )
                {
                    if( myAch.apiname == gAch.ApiName )
                    {
                        var myUserAch = new UserAchievement( gAch, myAch );
                        myUserAch.OwnerId = myUserId;
                        myAchievements.Add( myUserAch );
                        _userAchievementRepository.AddOrUpdate( myUserAch );
                    }
                }
            }
        }

        var theirRawAchievementData = new AchievementRoot();
        theirRawAchievementData = _steamService.GetAchievements( friendSteamId, appId );
        
        var theirAchievements = new List<UserAchievement>();
        foreach( var gAch in gameAchievements )
        {
            foreach( var theirAch in theirRawAchievementData.playerstats.achievements )
            {
                if( theirAch.apiname == gAch.ApiName )
                {
                    var theirUserAch = new UserAchievement( gAch, theirAch );
                    theirAchievements.Add( theirUserAch );
                }
            }
        }

        var viewModel = new CompeteVM( myAchievements, theirAchievements );

        return View( viewModel );
    }
}
