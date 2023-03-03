
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.ViewModels;
using SteamProject.Services;

namespace SteamProject.Controllers;

public class FriendController : Controller 
{
    private readonly ILogger<FriendController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IFriendRepository _friendRepository;
    private readonly ISteamService _steamService;

    public FriendController(
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
    public IActionResult Index( string friendSteamId )
    {
        var id = _userManager.GetUserId( User );
        var currentUser = _userRepository.GetUser( id );
        var friend = _friendRepository.GetSpecificFriend( friendSteamId );

        var myGames = _steamService.GetGames( currentUser.SteamId, currentUser.Id );
        var theirGames = _steamService.GetGames( friendSteamId, 0 );

        var sharedGames = myGames.Join(theirGames, g1 => g1.AppId, g2 => g2.AppId, (g1, g2) => g1 ).ToList<Game>();

        var viewModel = new FriendsVM( sharedGames, friend );
        viewModel.Id = currentUser.Id;
        viewModel.SteamId = currentUser.SteamId;
        
        return View( viewModel );


        // return View( _gameRepository.GetAll().Where( g => g.OwnerId == CurrentUser.Id ).ToList<Game>() );
    }
    
}