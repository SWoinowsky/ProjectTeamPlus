
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
    private readonly ICompetitionRepository _competitionRepository;
    private readonly ICompetitionPlayerRepository _competitionPlayerRepository;
    private readonly ICompetitionGameAchievementRepository _competitionGameAchievementRepository;
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
        ,ICompetitionRepository competitionRepository
        ,ICompetitionPlayerRepository competitionPlayerRepository
        ,ICompetitionGameAchievementRepository competitionGameAchievementRepository
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
        _competitionRepository = competitionRepository;
        _competitionPlayerRepository = competitionPlayerRepository;
        _competitionGameAchievementRepository = competitionGameAchievementRepository;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    [HttpGet]
    public IActionResult Initiate( string friendSteamId, int appId )
    {        
        var id = _userManager.GetUserId( User );
        var me = _userRepository.GetUser( id );
        var mySteamId = me.SteamId;
        var myUserId = me.Id;

        // making sure the game is actually there, we'll run into problems otherwise
        var gameIsInDb = false; 
        gameIsInDb = _gameRepository.GetGameByAppId( appId ) != null;
        if( !gameIsInDb )
        {
            var gamesToAdd = new List<Game>();
            gamesToAdd = _steamService.GetGames( mySteamId, myUserId ).ToList<Game>();
            foreach( var game in gamesToAdd )
            {
                _gameRepository.AddOrUpdate( game );
            }
        }

        var gameIdFound = _gameRepository.GetGameByAppId(appId).Id;

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
        myAchievements = _userAchievementRepository.GetAchievementsByGameAndUserId( gameIdFound, myUserId ).Where( a => a.Achieved == false ).ToList<UserAchievement>();
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
                        if( !myUserAch.Achieved )
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
                    if( !theirUserAch.Achieved )
                        theirAchievements.Add( theirUserAch );
                }
            }
        }

        var sharedMissingAchievements = new List<UserAchievement>();
        foreach( var myAch in myAchievements )
        {
            foreach( var theirAch in theirAchievements )
            {
                if( myAch.AchievementId == theirAch.AchievementId)
                {
                    if( myAch.Achieved == theirAch.Achieved )
                        sharedMissingAchievements.Add(myAch);
                }    
                
            }
        }
        myAchievements = sharedMissingAchievements;
        theirAchievements = myAchievements;

        var viewModel = new CompeteInitiateVM( myAchievements, theirAchievements );

        var compsWithUser = new List<CompetitionPlayer>();
        compsWithUser = _competitionPlayerRepository.GetCompetitionIdsBySteamId( mySteamId );

        var existingCompetition = new Competition();
        foreach( var compPlayer in compsWithUser )
        {
            existingCompetition = _competitionRepository.GetCompetitionByCompPlayerAndGameId( compPlayer, gameIdFound );
            
            if( existingCompetition != null )
                break;
        }
        
        viewModel.MySteamId = mySteamId;
        viewModel.CurrentCompetition = existingCompetition;


        return View( viewModel );
    }

    [Authorize]
    [HttpPost]
    public IActionResult Initiate( string friendSteamId, int appId, CompeteInitiateVM competeIn)
    {
        _competitionRepository.AddOrUpdate( competeIn.CurrentCompetition );
        foreach( var achievement in competeIn.UsersAchievements )
        {
            var objectOut = new CompetitionGameAchievement { CompetitionId = competeIn.CurrentCompetition.Id, GameAchievementId = achievement.AchievementId };
            _competitionGameAchievementRepository.AddOrUpdate(objectOut);
        }

        var compPlayerMe = new CompetitionPlayer { CompetitionId = competeIn.CurrentCompetition.Id, SteamId = competeIn.MySteamId };
        var compPlayerThem = new CompetitionPlayer { CompetitionId = competeIn.CurrentCompetition.Id, SteamId = friendSteamId };

        _competitionPlayerRepository.AddOrUpdate( compPlayerMe );
        _competitionPlayerRepository.AddOrUpdate( compPlayerThem );
        
        return RedirectToAction("Initiate", new { friendSteamId = friendSteamId, appId = appId });
    }
}
