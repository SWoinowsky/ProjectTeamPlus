
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.Services;

namespace SteamProject.Controllers;

public class FriendController : Controller 
{
    private readonly ILogger<FriendController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly ISteamService _steamService;

    public FriendController(
        ILogger<FriendController> logger
        ,ISteamService steamService
        ,IUserRepository userRepository
        ,IGameRepository gameRepository
        ,UserManager<IdentityUser> userManager
        )
    {
        _logger = logger;
        _steamService = steamService;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Index( string friendSteamId )
    {
        var id = _userManager.GetUserId( User );
        var CurrentUser = _userRepository.GetUser( id );

        var MyGames = _steamService.GetGames( CurrentUser.SteamId, CurrentUser.Id );
        var TheirGames = _steamService.GetGames( friendSteamId, 0 );

        var SharedGames = MyGames.Join(TheirGames, g1 => g1.AppId, g2 => g2.AppId, (g1, g2) => g1 ).ToList<Game>();
        return View( SharedGames );


        // return View( _gameRepository.GetAll().Where( g => g.OwnerId == CurrentUser.Id ).ToList<Game>() );


    }
    
}