using Microsoft.AspNetCore.Mvc;
using SteamProject.Models;
using SteamProject.Services;
using System.Diagnostics;
using System.Text.Json;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using Microsoft.AspNetCore.Identity;
using SteamProject.Models.DTO;
using Microsoft.AspNetCore.Identity.UI.Services;

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

    public SteamController(UserManager<IdentityUser> userManager, IUserRepository userRepository, ISteamService steamService, IGameRepository gameRepository, IUserGameInfoRepository userGameInfoRepository, IEmailSender emailSender)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _steamService = steamService;
        _gameRepository = gameRepository;
        _userGameInfoRepository = userGameInfoRepository;
        _emailSender = emailSender;
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

    [HttpGet("schema")]
    public ActionResult<SchemaRoot> GameSchema(int appId)
    {
        return _steamService.GetSchema(appId);
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
        var game = _userGameInfoRepository.GetAll().First(g => g.Game.AppId == Int32.Parse(id));

        if (game.Followed != true)
        {
            game.Followed = true;
        }
        else
        {
            game.Followed = false;
        }
        
        _userGameInfoRepository.AddOrUpdate(game);
        return Ok();
    }

    [HttpPost("unfollow")]
    public ActionResult UnFollow(string id)
    {
        //Try parse instead of Int32
        var game = _userGameInfoRepository.GetAll().First(g => g.Game.AppId == Int32.Parse(id));
        if (game.Followed != true)
        {
            return Ok();
        }
        else
        {
            game.Followed = false;
        }
        _userGameInfoRepository.AddOrUpdate(game);
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

    [HttpPost("sendInvite")]
    public ActionResult SendInvite(string email)
    {
        _emailSender.SendEmailAsync(email: email, subject: "Invitation", htmlMessage: "");
        Console.WriteLine(email);
        return Ok();
    }
}
