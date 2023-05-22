
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
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
    private readonly IStatusRepository _statusRepository;
    private readonly ISpeedRunRepository _speedRunRepository;
    private readonly IGameVoteRepository _gameVoteRepository;

    public CompeteController(
        ILogger<FriendController> logger
        ,ISteamService steamService
        , IUserRepository userRepository
        , IFriendRepository friendRepository
        , IGameAchievementRepository gameAchievementRepository
        , IUserAchievementRepository userAchievementRepository
        , IGameRepository gameRepository
        , IUserGameInfoRepository userGameInfoRepository
        , ICompetitionRepository competitionRepository
        , ICompetitionPlayerRepository competitionPlayerRepository
        , ICompetitionGameAchievementRepository competitionGameAchievementRepository
        , UserManager<IdentityUser> userManager
        , IInboxService inboxService
        , IStatusRepository statusRepository
        , ISpeedRunRepository speedRunRepository
        , IGameVoteRepository gameVoteRepository
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
        _statusRepository = statusRepository;
        _speedRunRepository = speedRunRepository;
        _gameVoteRepository = gameVoteRepository;
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
        viewModel.CompsComplete = new List<Competition>();
        viewModel.CompsRunning = new List<Competition>();

        if ( viewModel.Competitions != null )
        {
            foreach (var competition in viewModel.Competitions)
            {
                competition.Game = _gameRepository.GetGameById(competition.GameId);
                if( DateTime.Compare( competition.EndDate, DateTime.Now ) <= 0 )
                    viewModel.CompsComplete.Add( competition );
                else
                    viewModel.CompsRunning.Add( competition );
            }
        }
        
        return View( viewModel );
    }

    [Authorize]
    public IActionResult Details(int compId)
    {
        var authId = _userManager.GetUserId(User);
        int SinId = _userRepository.GetUser(authId).Id;
        var currentUser = _userRepository.GetUser(authId);

        var viewModel = new CompeteDetailsVM();
        var competitionIn = new Competition();



        viewModel.SinId = SinId;

        competitionIn = _competitionRepository.GetCompetitionById(compId);


        if (competitionIn != null)
        {
            var compPlayersList = new List<CompetitionPlayer>();
            compPlayersList = _competitionPlayerRepository.GetAllForCompetition(compId);

            var compAchievements = new List<CompetitionGameAchievement>();

            if (DateTime.UtcNow >= competitionIn.EndDate)
            {
                // Competition has ended, check if the vote has succeeded
                bool hasVoteSucceeded = _competitionRepository.HasVoteSucceeded(competitionIn.Id);

                if (hasVoteSucceeded)
                {
                    bool hasGameVoteSucceeded = _gameVoteRepository.HasGameVoteSucceeded(compId);
                    if (hasGameVoteSucceeded)
                    {
                        var newGameId = _gameVoteRepository.GetGameIdWithMostVotes(compId);

                        // Update the existing competition with the new game and updated dates
                        competitionIn.GameId = newGameId;

                        // Calculate the original competition duration
                        var competitionDuration = competitionIn.EndDate - competitionIn.StartDate;

                        // Set the end date to the start date + the original competition duration
                        competitionIn.EndDate = competitionIn.StartDate + competitionDuration;

                        //the end date needs to be set to the start date + the number of days the competition was active
                        competitionIn.EndDate = competitionIn.EndDate.AddDays((competitionIn.EndDate - competitionIn.StartDate).TotalDays);
                        competitionIn.StatusId = 1; // set to the default status ID which is active

                        _competitionRepository.AddOrUpdate(competitionIn);

                        // Fetch the new game's achievements
                        _gameAchievementRepository.EnsureGameAchievements(competitionIn.Game.AppId, currentUser.SteamId, currentUser.Id);
                        _competitionGameAchievementRepository.EnsureCompetitionGameAchievements(compId, competitionIn.GameId);
                        compAchievements = _competitionGameAchievementRepository.GetByCompetitionIdAndGameId(compId, competitionIn.Game.Id);
                    }
                    else
                    {
                        var gameSelectionStatus = _statusRepository.GetStatusByName("GameSelection");
                        if (gameSelectionStatus != null)
                        {
                            competitionIn.Status = gameSelectionStatus;
                            _competitionRepository.AddOrUpdate(competitionIn);

                            // Clear achievements for GameSelection status or fetch default achievements if there are any
                            compAchievements = new List<CompetitionGameAchievement>();
                        }
                    }
                }
                else if (competitionIn.Status.Name != "Ended")
                {
                    var endedStatus = _statusRepository.GetStatusByName("Ended");
                    if (endedStatus != null)
                    {
                        competitionIn.Status = endedStatus;
                        _competitionRepository.AddOrUpdate(competitionIn);
                    }
                }
            }
            else
            {
                // Competition has not ended, fetch current game's achievements
                compAchievements = _competitionGameAchievementRepository.GetByCompetitionIdAndGameId(compId, competitionIn.Game.Id);
            }

            var gameAssociated = new Game();
            gameAssociated = _gameRepository.GetGameById( competitionIn.GameId );

            // List of steamids of competition's associated steam users. Feeds into GetManyUsers function.
            var idList = new List<string>();
            foreach( var cPlayer in compPlayersList )
            {
                idList.Add( cPlayer.SteamId );
            }
            var userList = new List<User>();
            userList = _steamService.GetManyUsers( idList );

            foreach(var run in _speedRunRepository.GetAll())
            {
                if(run.CompetitionId == compId)
                {
                    return RedirectToAction("SpeedRunDetails", new {compId = compId});
                }
            }
            if(compAchievements == null && competitionIn.Goal != null || compAchievements.Count() < 1 && competitionIn.Goal != null)
            {
                return RedirectToAction("SpeedRunDetails", new {compId = compId});
            }

            _gameAchievementRepository.EnsureGameAchievements(gameAssociated.AppId, currentUser.SteamId, currentUser.Id);

            var percentages = new List<GlobalAchievement>();
            percentages = _steamService.GetGAP( competitionIn.Game.AppId ).achievementpercentages.achievements;
            
            var gameAchievements = new List<GameAchievement>();
            foreach( var ach in compAchievements )
            {

                var achievementFound = new GameAchievement();
                achievementFound = _gameAchievementRepository.GetAll().Where( gAch => gAch.Id == ach.GameAchievementId ).FirstOrDefault();

                if( achievementFound != null )
                    gameAchievements.Add( achievementFound );
            }

            var pointProcessor = new GapProcessor();
            foreach( var gameAch in gameAchievements )
            {
                foreach( var percent in percentages )
                {
                    if( gameAch.ApiName == percent.name )
                    {
                        gameAch.PointVal = pointProcessor.HandlePercent( percent.percent );
                    }
                }
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
                        
                        if( userAchOut.Achievement.DisplayName == "Lucatiel" && participant.SteamId == "76561198069530799" )
                        {
                            userAchOut.Achieved = true;
                            userAchOut.UnlockTime = new DateTime(2023, 5, 12, 12, 27, 00, DateTimeKind.Local);
                        }

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

            var userScoreList = new List<KeyValuePair<User, CompetitionPlayer>>();
            foreach( var player in compPlayersList )
            {
                foreach( var user in userList )
                {
                    if( player.SteamId == user.SteamId )
                        userScoreList.Add( new (user, player) );
                }
            }

            
            userScoreList = userScoreList.OrderByDescending( i => i.Value.Score ).ThenBy( i => i.Key.SteamName ).ToList<KeyValuePair<User, CompetitionPlayer>>();

            userList.Clear();
            foreach( var us in userScoreList )
            {
                userList.Add( us.Key );
                foreach( var ua in userAchList )
                {
                    if( us.Key == ua.Value )
                    {
                        us.Value.Score += ua.Key.Achievement.PointVal;
                    }
                }
            }


            


            viewModel.CurrentComp = competitionIn;
            viewModel.Game = gameAssociated;
            viewModel.CompPlayers = compPlayersList;
            viewModel.Players = userList;
            viewModel.CompGameAchList = compAchievements;
            viewModel.GameAchList = gameAchievements;
            viewModel.Tracking = userAchList;
            viewModel.Scoreboard = userScoreList;
            viewModel.Vote = competitionIn.CompetitionVotes.Where( v => v.UserId == SinId ).FirstOrDefault();
            viewModel.Status = competitionIn.Status;
        }

        return View( viewModel );
    }

    [Authorize]
    [HttpPost]
    public IActionResult SelectGame(int compId, int gameId)
    {
        var competition = _competitionRepository.GetCompetitionById(compId);
        var game = _gameRepository.GetGameByAppId(gameId);
        if (competition == null || game == null)
        {
            return NotFound();
        }

        if (competition.Status.Name != "GameSelection")
        {
            return BadRequest("Competition is not in the game selection state.");
        }

        competition.GameId = game.Id;
        competition.Game = game;
        competition.Status = _statusRepository.GetStatusByName("GameSelected");
        _competitionRepository.AddOrUpdate(competition);
        _inboxService.SendMessage(competition.CreatorId, 69420, $"Game {competition.Game.Name} has been selected for competition!");

        return RedirectToAction("SetupCompetition", new { compId = competition.Id });
    }

    [Authorize]
    [HttpPost]
    public IActionResult SetupCompetition(int compId, DateTime startTime, DateTime endTime)
    {
        var competition = _competitionRepository.GetCompetitionById(compId);
        if (competition == null)
        {
            return NotFound();
        }

        if (competition.Status.Name != "GameSelected")
        {
            return BadRequest("Competition is not in the correct state for setup.");
        }

        competition.StartDate = startTime;
        competition.EndDate = endTime;
        competition.Status = _statusRepository.GetStatusByName("Active");
        _competitionRepository.AddOrUpdate(competition);

        return RedirectToAction("Details", new { compId = competition.Id });
    }


    [Authorize]
    [HttpGet]
    public IActionResult SpeedRunDetails( int compId )
    {
        var authId = _userManager.GetUserId(User);
        var temp = _userRepository.GetUser( authId);
        int SinId = _userRepository.GetUser( authId ).Id;

        var viewModel = new CompeteDetailsVM();
        var competitionIn = new Competition();

        viewModel.SinId = SinId;

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

            if (DateTime.UtcNow >= competitionIn.EndDate && competitionIn.Status.Name != "Ended")
            {
                var endedStatus = _statusRepository.GetStatusByName("Ended");
                if (endedStatus != null)
                {
                    competitionIn.Status = endedStatus;
                    _competitionRepository.AddOrUpdate(competitionIn);
                } 
            }

            string id = _userManager.GetUserId(User);

            var currentUser = new SteamProject.Models.User();
            currentUser = _userRepository.GetUser(id);

            var compRuns = _speedRunRepository.GetAllSpeedRunsForComp(compId);
            
            if(compRuns.Count() > 0)
            {
                List<SpeedRun> fastestRuns = new List<SpeedRun>();
                List<SpeedRun> slowestRuns = new List<SpeedRun>();

                Dictionary<User, SpeedRun> fastestRunByPlayer = new Dictionary<User, SpeedRun>();
                List<KeyValuePair<User, SpeedRun>> slowestRunsAllPlayers = new List<KeyValuePair<User, SpeedRun>>();

                foreach(var run in compRuns)
                {
                    if(run.Fastest)
                    {
                        fastestRuns.Add(run);
                    }
                    else
                    {
                        slowestRuns.Add(run);
                    }
                }

                fastestRuns = fastestRuns.OrderBy(run => TimeSpan.Parse(run.RunTime)).ToList();
                slowestRuns = slowestRuns.OrderBy(run => TimeSpan.Parse(run.RunTime)).ToList();
                foreach(var run in fastestRuns)
                {
                    foreach(var player in compPlayersList)
                    {
                        if(player.SteamId == run.SteamId)
                        {
                            var user = userList.Where(u => u.SteamId == player.SteamId).Single();
                            fastestRunByPlayer.Add(user, run);
                            break;
                        }
                    }
                }
                foreach(var run in slowestRuns)
                {
                    foreach(var player in compPlayersList)
                    {
                        if(player.SteamId == run.SteamId)
                        {
                            var user = userList.Where(u => u.SteamId == player.SteamId).Single();
                            slowestRunsAllPlayers.Add(new KeyValuePair<User, SpeedRun>(user, run));
                            break;
                        }
                    }
                }
                viewModel.FastestRuns = fastestRunByPlayer;
                viewModel.SlowestRuns = slowestRunsAllPlayers;
            }

            viewModel.CurrentComp = competitionIn;
            viewModel.Game = gameAssociated;
            viewModel.CompPlayers = compPlayersList;
            viewModel.Players = userList;
            foreach(var player in _competitionPlayerRepository.GetAll())
            {
                if(player.SteamId == currentUser.SteamId)
                {
                    viewModel.SteamId = currentUser.SteamId;
                    viewModel.CurrentUserId = player.Id;
                    break;
                }
            }
        }
        return View( viewModel );
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
    [HttpGet]
    public IActionResult CreateSpeedRun()
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
        
        //refactor to save these values to the viewmodel to make it easier to pass around
        viewModel.MySteamId = mySteamId;
        viewModel.MyFriendId = friendSteamId;
        viewModel.CurrentCompetition = existingCompetition;
        
        return View( viewModel );
    }

    /// <summary>
    /// Function to create competitions from the profile page by clicking on friends name, currently IN-OP
    /// Before I worked on this the viewmodel was failing to bind when posted back here, they now bind correctly but this function still needs some work if we choose to use it still
    /// -Cole
    /// </summary>
    /// <param name="competeIn"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    public IActionResult Initiate(CompeteInitiateVM competeIn)
    {
        //This method is broken and will not save compititons in this format anymore, I tried refactoring it some but its gonna need some more work to get going.

        competeIn.ChosenGame = _gameRepository.GetGameById(competeIn.ChosenGame.Id);

        //Fails on saving competitions to the database here due to some of the values being null
        _competitionRepository.AddOrUpdate(competeIn.CurrentCompetition);
        foreach (var achievement in competeIn.UsersAchievements)
        {
            var objectOut = new CompetitionGameAchievement { CompetitionId = competeIn.CurrentCompetition.Id, GameAchievementId = achievement.AchievementId };
            _competitionGameAchievementRepository.AddOrUpdate(objectOut);
        }

        var compPlayerMe = new CompetitionPlayer { CompetitionId = competeIn.CurrentCompetition.Id, SteamId = competeIn.MySteamId };
        var compPlayerThem = new CompetitionPlayer { CompetitionId = competeIn.CurrentCompetition.Id, SteamId = competeIn.MyFriendId };

        _competitionPlayerRepository.AddOrUpdate(compPlayerMe);
        _competitionPlayerRepository.AddOrUpdate(compPlayerThem);

        return RedirectToAction("Initiate", new { SteamId = competeIn.MyFriendId, appId = competeIn.ChosenGame.AppId });
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create( CompeteCreateVM compCreatedOut )
    {
        string authid = _userManager.GetUserId(User);
        var SinId = _userRepository.GetUser( authid ).Id;

        var timeString = compCreatedOut.MinDate.ToString();
        var game = new Game();
        game = _gameRepository.GetGameByAppId( compCreatedOut.GameAppId );

        var comp = new Competition()
        {
            CreatorId = SinId,
            GameId = game.Id,
            StartDate = compCreatedOut.CompStartTime,
            EndDate = compCreatedOut.CompEndTime,
            Game = game,
        };

        comp.Status = _statusRepository.GetStatusByName("Active");

        _competitionRepository.AddOrUpdate( comp );


        _inboxService.SendMessage(SinId, 69420, $"You started a new achievement competition for {comp.Game.Name}! Starting on {comp.StartDate.ToLocalTime()} and finishing {comp.EndDate.ToLocalTime()}");

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

        return RedirectToRoute("Compete", new { controller = "Compete", action = "Details", compId = comp.Id });

    }

    [Authorize]
    [HttpPost]
    public IActionResult CreateSpeedRun( CompeteCreateVM compCreatedOut )
    {
        string authid = _userManager.GetUserId(User);
        var SinId = _userRepository.GetUser( authid ).Id;

        var timeString = compCreatedOut.MinDate.ToString();
        var game = new Game();
        game = _gameRepository.GetGameByAppId( compCreatedOut.GameAppId );
        string goal = compCreatedOut.Goal;

        var comp = new Competition()
        {
            CreatorId = SinId,
            GameId = game.Id,
            StartDate = compCreatedOut.CompStartTime,
            EndDate = compCreatedOut.CompEndTime,
            Game = game,
            Goal = goal
        };

        comp.Status = _statusRepository.GetStatusByName("Active");

        _competitionRepository.AddOrUpdate( comp );

        _inboxService.SendMessage(SinId, 69420, $"You started a new speed run competition for {comp.Game.Name}! Starting on {comp.StartDate.ToLocalTime()} and finishing {comp.EndDate.ToLocalTime()}");

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

        
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult SubmitRun(string glitch, string time, string youtubeLink, string playerId, string steamId, string compId)
    {
        var playerRunsExist = false;
        var run = new SpeedRun ()
        {
            GlitchStatus = (glitch == "glitched") ? true : false,
            RunTime = time,
            VideoId = string.IsNullOrEmpty(youtubeLink) ? string.Empty :
                youtubeLink.Substring(youtubeLink.IndexOf("?v=") + 3,
                (youtubeLink.IndexOf("&") == -1) ? youtubeLink.Length : youtubeLink.IndexOf("&") - (youtubeLink.IndexOf("?v=") + 3)),
            CompetitionId = Int32.Parse(compId),
            PlayerId = Int32.Parse(playerId),
            SteamId = steamId
        };
        var runsForCurrentComp = _speedRunRepository.GetAllSpeedRunsForComp(Int32.Parse(compId));
        foreach(var comp in runsForCurrentComp)
        {
            if(comp.PlayerId == Int32.Parse(playerId))
            {
                playerRunsExist = true;
            }
        }
        if(runsForCurrentComp.Count() == 0 || playerRunsExist == false)
        {
            run.Fastest = true;
            _speedRunRepository.AddOrUpdate(run);
        }
        else
        {
            var NewRunTime = TimeSpan.Parse(run.RunTime);
            foreach(var comp in runsForCurrentComp)
            {
                var CompRunTime = TimeSpan.Parse(comp.RunTime);
                if(comp.Fastest == true && run.PlayerId == comp.PlayerId)
                {
                    if(NewRunTime < CompRunTime || NewRunTime == CompRunTime)
                    {
                        run.Fastest = true;
                        comp.Fastest = false;
                        _speedRunRepository.AddOrUpdate(run);
                        return RedirectToAction("SpeedRunDetails", new RouteValueDictionary {{"compId", compId}});
                    }
                    break;
                }
            }
            run.Fastest = false;
            _speedRunRepository.AddOrUpdate(run);
        }

        return RedirectToAction("SpeedRunDetails", new RouteValueDictionary {{"compId", compId}});
    }
}
