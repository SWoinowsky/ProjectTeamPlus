using Microsoft.AspNetCore.Mvc;
using SteamProject.Models;
using SteamProject.Services;
using System.Diagnostics;
using System.Text.Json;


namespace SteamProject.Controllers;

[Route("api/[controller]")]
[ApiController]

public class SteamController : ControllerBase
{
    private readonly ISteamService _steamService;
    private readonly ISteamServices _steamServices;

    public SteamController( ISteamService steamService, ISteamServices steamServices )
    {
        _steamService = steamService;
        _steamServices = steamServices;
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
        return _steamServices.GetAchievements(steamid, appId);
    }

    [HttpGet("schema")]
    public ActionResult<SchemaRoot> GameSchema(int appId)
    {
        return _steamServices.GetSchema(appId);
    }
}
