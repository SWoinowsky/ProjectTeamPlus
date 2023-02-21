using Microsoft.AspNetCore.Mvc;
using SteamProject.Models;
using SteamProject.Services;
using System.Diagnostics;
using System.Text.Json;
using SteamProject.DAL.Abstract;


namespace SteamProject.Controllers;

[Route("api/[controller]")]
[ApiController]

public class SteamController : ControllerBase
{
    private readonly ISteamService _steamService;
    private readonly IGameRepository _gameRepository;

    public SteamController( ISteamService steamService, IGameRepository gameRepository )
    {
        _steamService = steamService;
        _gameRepository = gameRepository;
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
    public ActionResult Hide(string id)
    {
        var game = _gameRepository.GetAll(g => g.AppId == Int32.Parse(id)).ToList()[0];
        game.Hidden = true;
        _gameRepository.AddOrUpdate(game);
        return Ok();
    }

    [HttpGet("friends")]
    public ActionResult SteamFriends(string steamid, int userId)
    {
        var listFriends = _steamService.GetFriendsList(steamid, userId);
        listFriends.OrderBy( x => x.Id );

        return Ok(listFriends);
    }
}
