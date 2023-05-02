
using System;
using System.Linq;
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
    private readonly IInboxService _inboxService;

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
        ,InboxService inboxService
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
        _inboxService = inboxService;
    }


    [Authorize]
    [HttpGet]
    public IActionResult Index()
    {
        var id = _userManager.GetUserId( User );
        var me = _userRepository.GetUser( id );
        var mySteamId = me.SteamId;

        var myEntries = new List<CompetitionPlayer>();
        myEntries = _competitionPlayerRepository.GetCompetitionIdsBySteamId( mySteamId );

        var viewModel = new CompeteIndexVM();
        viewModel.Competitions = _competitionRepository.GetAllCompetitionsForUser( myEntries );

        if ( viewModel.Competitions != null )
        {
            foreach (var competition in viewModel.Competitions)
            {
                competition.Game = _gameRepository.GetGameById(competition.GameId);
            }
        }
        
        return View( viewModel );
    }

    [Authorize]
    [HttpGet]
    public IActionResult Details( int compId )
    {
        var viewModel = new CompeteDetailsVM();
        var competitionIn = new Competition();

        competitionIn = _competitionRepository.GetCompetitionById( compId );

        if( competitionIn != null )
        {
            var gameAssociated = new Game();
            gameAssociated = _gameRepository.GetGameById( competitionIn.GameId );


            var compPlayersList = new List<CompetitionPlayer>();
            compPlayersList = _competitionPlayerRepository.GetAllForCompetition( compId );


            // List of steamids of competition's associated steam users. Feeds into GetManyUsers function.
            var idList = new List<string>();
            foreach( var cPlayer in compPlayersList )
            {
                idList.Add( cPlayer.SteamId );
            }
            var userList = new List<User>();
            userList = _steamService.GetManyUsers( idList );


            var compAchievements = new List<CompetitionGameAchievement>();
            compAchievements = _competitionGameAchievementRepository.GetByCompetitionId( compId );

            
            var gameAchievements = new List<GameAchievement>();
            foreach( var ach in compAchievements )
            {
                var achievementFound = new GameAchievement();
                achievementFound = _gameAchievementRepository.GetAll().Where( gAch => gAch.Id == ach.GameAchievementId ).FirstOrDefault();

                if( achievementFound != null )
                    gameAchievements.Add( achievementFound );
            }
            

            // Participant achievement grabbing
            var userAchDict = new Dictionary<UserAchievement, User>();
            foreach( var participant in userList )
            {
                var ListIntoDict = new List<UserAchievement>();

                var userResponse = new AchievementRoot();
                userResponse = _steamService.GetAchievements( participant.SteamId, gameAssociated.AppId );

                if( userResponse != null )
                    foreach( var ach in gameAchievements )
                    {
                        var userAchOut = new UserAchievement();
                        userAchOut = userAchOut.GetUserAchievementFromAPICall( ach, userResponse.playerstats.achievements );
                        if( userAchOut != null  && userAchOut.Achieved == true && userAchOut.AchievedWithinWindow( competitionIn ))
                            ListIntoDict.Add( userAchOut );
                    }

                foreach( var achievement in ListIntoDict )
                {
                    userAchDict.Add( achievement, participant );
                }
            }

            var userAchList = new List<KeyValuePair<UserAchievement, User>>();
            userAchList = userAchDict.OrderByDescending( p => p.Key.UnlockTime ).ToList<KeyValuePair<UserAchievement, User>>();
            
            viewModel.CurrentComp = competitionIn;
            viewModel.Game = gameAssociated;
            viewModel.CompPlayers = compPlayersList;
            viewModel.Players = userList;
            viewModel.CompGameAchList = compAchievements;
            viewModel.GameAchList = gameAchievements;
            viewModel.Tracking = userAchList;
        }

        return View( viewModel );
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

        var gameInUse = new Game();
        gameInUse = _gameRepository.GetGameById( gameIdFound );

        viewModel.ChosenGame = gameInUse;

        var compsWithUser = new List<CompetitionPlayer>();
        compsWithUser = _competitionPlayerRepository.GetCompetitionIdsBySteamId( mySteamId );

        var existingCompetition = new Competition();
        if( compsWithUser != null && compsWithUser.Count() != 0 )
        {
            foreach( var compPlayer in compsWithUser )
            {
                existingCompetition = _competitionRepository.GetCompetitionByCompPlayerAndGameId( compPlayer, gameIdFound );
                
                if( existingCompetition != null )
                    break;
            }
        }
        else {
            existingCompetition = null;
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


    [Authorize]
    [HttpGet]
    public IActionResult Create()
    {
        string id = _userManager.GetUserId(User);

        var currentUser = new SteamProject.Models.User();
        currentUser = _userRepository.GetUser(id);

        var viewModel = new CompeteCreateVM();
        viewModel.SteamId = currentUser.SteamId;
        viewModel.SinId = currentUser.Id;
        

        return View( viewModel );
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create( CompeteCreateVM compCreatedOut )
    {
        var timeString = compCreatedOut.MinDate.ToString();
        var game = new Game();
        game = _gameRepository.GetGameByAppId( compCreatedOut.GameAppId );

        var comp = new Competition()
        {
            GameId = game.Id,
            StartDate = compCreatedOut.CompStartTime,
            EndDate = compCreatedOut.CompEndTime,
            Game = game,
        };

        _competitionRepository.AddOrUpdate( comp );


        var competitors = new List<CompetitionPlayer>();
        competitors.Add( new CompetitionPlayer() { CompetitionId = comp.Id, SteamId = compCreatedOut.SteamId } );
        
        if( compCreatedOut.OpponentId != null )
            competitors.Add( new CompetitionPlayer() { CompetitionId = comp.Id, SteamId = compCreatedOut.OpponentId } );
        
        if( compCreatedOut.OpponentIds != null )
            foreach( string id in compCreatedOut.OpponentIds )
            {
                competitors.Add( new CompetitionPlayer() { CompetitionId = comp.Id, SteamId = id, Competition = comp } );
            }

        foreach( CompetitionPlayer opp in competitors )
        {
            _competitionPlayerRepository.AddOrUpdate( opp );
        }

        var presentAchievements = new List<GameAchievement>();
        presentAchievements = _gameAchievementRepository.GetAchievementsFromGameId( game.Id );        

        if( presentAchievements.Count() <= 0 )
        {
            var achievementList = new List<SchemaAchievement>();
            achievementList = _steamService.GetSchema( game.AppId ).game.availableGameStats.achievements;

            foreach( SchemaAchievement ach in achievementList )
            {   
                var newAch = new GameAchievement( ach );
                _gameAchievementRepository.AddOrUpdate( newAch );
                presentAchievements.Add( newAch );
            }
        }

        var achievementsCompeting = new List<GameAchievement>();
        foreach( var achUnearned in compCreatedOut.AchievementDisplayNames )
        {
            foreach( var gameAch in presentAchievements )
            {
                if( achUnearned == gameAch.DisplayName )
                    achievementsCompeting.Add( gameAch );
            }
        }

        foreach( GameAchievement ach in achievementsCompeting )
        {
            var compAch = new CompetitionGameAchievement() { CompetitionId = comp.Id, GameAchievementId = ach.Id };
            _competitionGameAchievementRepository.AddOrUpdate( compAch );
        }

        return View( compCreatedOut );
    }

    
}
