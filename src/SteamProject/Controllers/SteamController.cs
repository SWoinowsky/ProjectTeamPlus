using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.Models.DTO;
using SteamProject.Services;

namespace SteamProject.Controllers;

[Route("api/[controller]")]
[ApiController]

public class SteamController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly ISteamService _steamService;
    private readonly IGameRepository _gameRepository;
    private readonly IUserGameInfoRepository _userGameInfoRepository;
    private readonly IEmailSender _emailSender;
    private readonly IFriendRepository _friendRepository;

    public SteamController(UserManager<IdentityUser> userManager, IUserRepository userRepository, ISteamService steamService, IGameRepository gameRepository, IUserGameInfoRepository userGameInfoRepository, IEmailSender emailSender, IFriendRepository friendRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _steamService = steamService;
        _gameRepository = gameRepository;
        _userGameInfoRepository = userGameInfoRepository;
        _emailSender = emailSender;
        _friendRepository = friendRepository;
    }


    [HttpGet("user")]
    public ActionResult SteamUser(string steamid)
    {
        User user = _steamService.GetSteamUser(steamid);

        return Ok(user);
    }

    [HttpGet("achievements")]
    public ActionResult<AchievementRoot> UserAchievements(string steamid, int appId)
    {
        return _steamService.GetAchievements(steamid, appId);
    }

    [HttpGet("sharedMissingAchievements")]
    public ActionResult SharedMissingAchievements( string userSteamId, string friendSteamId, int appId )
    {
        return Ok(_steamService.GetSharedMissingAchievements( userSteamId, friendSteamId, appId ));
    }

    [HttpGet("schema")]
    public ActionResult<SchemaRoot> GameSchema(int appId)
    {
        return _steamService.GetSchema(appId);
    }

    [HttpGet("games")]
    public ActionResult Games( string userSteamId, int userId )
    {
        return Ok(_steamService.GetGames(userSteamId, userId) );
    }

    [HttpGet("sharedGames")]
    public ActionResult SharedGames( string userSteamId, string friendSteamId, int userId )
    {
        return Ok( _steamService.GetSharedGames( userSteamId, friendSteamId, userId ) );
    }
    
    [HttpPost("hide")]
    public ActionResult Hide(string id, string userId)
    {
        UserGameInfo game = new UserGameInfo();
        game = game.GetGameByIdAndUser(Int32.Parse(id), _userGameInfoRepository, Int32.Parse(userId));
        game.SetHiddenStatusTrue();
        _userGameInfoRepository.AddOrUpdate(game);
        return Ok();
    }

    [HttpPost("unhide")]
    public ActionResult Unhide(string id, string userId)
    {
        UserGameInfo game = new UserGameInfo();
        game = game.GetGameByIdAndUser(Int32.Parse(id), _userGameInfoRepository, Int32.Parse(userId));
        game.SetHiddenStatusFalse();
        _userGameInfoRepository.AddOrUpdate(game);
        return Ok();
    }

    [HttpGet("refresh")]
    public ActionResult RefreshLibrary()
    {
        var routeValues = new RouteValueDictionary {
            {"refresh", true}
        };
        return RedirectToAction("Index", "Library", routeValues);
    }

    [HttpPost("follow")]
    public ActionResult ToggleFollow(string id)
    {
        string? userid = _userManager.GetUserId(User);

        if (id is null)
        {
            return BadRequest();
        }
        else
        {
            User user = _userRepository.GetUser(userid);

            if (user.SteamId != null)
            {

                var game = _userGameInfoRepository.GetAll().Where(o => o.OwnerId == user.Id)
                    .First(g => g.Game.AppId == Int32.Parse(id));

                if (game.Followed != true)
                {
                    game.Followed = true;
                }
                else
                {
                    game.Followed = false;
                }

                _userGameInfoRepository.AddOrUpdate(game);
            }
        }

        return Ok();
    }

    [HttpPost("unfollow")]
    public ActionResult UnFollow(string id)
    {
        string? userid = _userManager.GetUserId(User);

        if (id is null)
        {
            return BadRequest();
        }
        else
        {
            User user = _userRepository.GetUser(userid);

            if (user.SteamId != null)
            {

                var game = _userGameInfoRepository.GetAll().Where(o => o.OwnerId == user.Id)
                    .First(g => g.Game.AppId == Int32.Parse(id));

                if (game.Followed == true)
                {
                    game.Followed = false;
                }

                _userGameInfoRepository.AddOrUpdate(game);
            }
        }

        return Ok();
    }

    [HttpGet("friends")]
    public ActionResult SteamFriends(string steamid, int userId)
    {
        var listFriends = _steamService.GetFriendsList(steamid, userId);
        listFriends.OrderBy( x => x.Id );

        return Ok(listFriends);
    }

    [HttpGet("friendSpecific")]
    public ActionResult SpecificFriend( string userSteamId, int userId, string friendSteamId )
    {
        var friend = _steamService.GetFriendSpecific( userSteamId, userId, friendSteamId );

        return Ok( friend );
    }

    [HttpGet("sendInvite")]
    public async Task<ActionResult> SendInvite(string email)
    {
        string fixedEmail = email.Replace("%40", "@");
        await _emailSender.SendEmailAsync($"{fixedEmail}", "Invitation", "<a>You're invited!</a>");
        return Ok();
    }

    [HttpPatch("setNickname")]
    public ActionResult SetNickname(string friendSteamId, string nickname)
    {   
        var friend = _friendRepository.GetSpecificFriend(friendSteamId);
        friend.Nickname = nickname;
        _friendRepository.AddOrUpdate(friend);
        return Ok();
    }

    [HttpPatch("revertNickname")]
    public ActionResult RevertNickname(string friendSteamId)
    {   
        var friend = _friendRepository.GetSpecificFriend(friendSteamId);
        friend.Nickname = null;
        _friendRepository.AddOrUpdate(friend);
        return Ok();
    }

    [HttpGet("gap")]
    public ActionResult<GAPRoot> GlobalPercentages(int appId)
    {
        return _steamService.GetGAP(appId);
    }

}
