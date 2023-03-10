
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Services;

namespace SteamProject.Controllers;

public class CompeteController : Controller
{
    private readonly ILogger<FriendController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IFriendRepository _friendRepository;
    private readonly ISteamService _steamService;

    public CompeteController(
        ILogger<FriendController> logger
        ,ISteamService steamService
        ,IUserRepository userRepository
        ,IFriendRepository friendRepository
        ,IGameRepository gameRepository
        ,UserManager<IdentityUser> userManager
        )
    {
        _logger = logger;
        _steamService = steamService;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _friendRepository = friendRepository;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Index( string friendSteamId, int appId )
    {
        var ViewString = $"Friend Id: {friendSteamId}; App Id: {appId}.";
        return View((object)ViewString);
    }
}
