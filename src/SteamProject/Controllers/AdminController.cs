using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using Microsoft.AspNetCore.Identity;
using SteamProject.Services;
using Microsoft.AspNetCore.Authorization;
using SteamProject.Models.DTO;
using SteamProject.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace SteamProject.Controllers;

[Authorize(Roles = "admin")]
public class AdminController: Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IBlackListRepository _blackListRepository;
    private readonly SignInManager<IdentityUser> _signInManager;
    private IFriendRepository _friendRepository;
    private IUserGameInfoRepository _userGameInfoRepository;
    private readonly ISteamService _steamService;
    private readonly IGameRepository _gameRepository;

    public AdminController(SignInManager<IdentityUser> signInManager, IGameRepository gameRepository, UserManager<IdentityUser> userManager, IUserRepository userRepository, IBlackListRepository blackListRepository, IUserGameInfoRepository userGameInfoRepository, IFriendRepository friendRepository, ISteamService steamService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _blackListRepository = blackListRepository;
        _friendRepository = friendRepository;
        _steamService = steamService;
        _userRepository = userRepository;
        _userGameInfoRepository = userGameInfoRepository;
        _gameRepository = gameRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ShowAllUsers()
    {
        List<AdminUsersVM> adminUsersVM = new List<AdminUsersVM>();

        foreach(var user in _userRepository.GetAllUsers())
        {
            var temp = _userManager.FindByIdAsync(user.AspNetUserId).Result;
            adminUsersVM.Add(new AdminUsersVM(){
                AspNetUserId = temp.Id,
                SteamId = user.SteamId,
                SteamName = user.SteamName,
                Email = temp.Email
            });
        }
        return View(adminUsersVM);
    }

    public async Task<IActionResult> Delete(string id)
    {
        var toBeBanned = new BlackList{
            SteamId = id
            };

        var loginProvider = "Steam";
        var providerKey = "https://steamcommunity.com/openid/id/" + id;

        var user = new IdentityUser();
        foreach(var tempUser in _userRepository.GetAllUsers())
        {
            if(tempUser.SteamId == id)
            {
                user = await _userManager.FindByIdAsync(tempUser.AspNetUserId);
                break;
            }
        }

        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
        if (!result.Succeeded)
        {
            return RedirectToAction(nameof(Index));
        }

        User currentUser = null;
        if (user != null)
        {
            if (user.Id != null)
            {
                currentUser = _userRepository.GetAll().FirstOrDefault(u => u.AspNetUserId == user.Id);
                var currentUserGameInfo = _userGameInfoRepository.GetAll().Where(g => g.OwnerId == currentUser.Id).ToList();
                var friendInfo = _friendRepository.GetAll().Where(f => f.RootId == currentUser.Id).ToList();

                if (currentUser != null)
                {
                    try
                    {
                        currentUser.SteamId = null;
                        currentUser.AvatarUrl = null;
                        currentUser.ProfileUrl = null;
                        currentUser.SteamName = null;
                        currentUser.PlayerLevel = null;
                        
                        currentUser.UserAchievements.Clear();

                        for (int i = 0; i < currentUserGameInfo.Count(); i++)
                        {
                            _userGameInfoRepository.Delete(currentUserGameInfo[i]);
                        }

                        for (int i = 0; i < friendInfo.Count(); i++)
                        {
                            _friendRepository.Delete(friendInfo[i]);
                        }
                        _userRepository.AddOrUpdate(currentUser);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
            }
        }

        // This is to make sure that somehow an ID doesn't get added to the blacklist multiple times.
        // A user will still have their linked account removed, the blacklist just isn't updated.
        foreach(var item in _blackListRepository.GetAll())
        {
            if(item.SteamId == id)
            {
                return RedirectToAction(nameof(ShowAllUsers));
            }
        }
        // If the blacklist doesn't have the id in it, then it's added here. User already had all data removed prior to this.
        _blackListRepository.AddOrUpdate(toBeBanned);
        return RedirectToAction(nameof(ShowAllUsers));
    }

    public IActionResult LoadGames()
    {
        IEnumerable<Game> games = _steamService.GetSteamCuratorGames();
        List<Game> returnGames = new List<Game>();
        foreach(var game in games)
        {
            try
            {
                var currentGame = _gameRepository.GetGameByAppId(game.AppId);
                if (currentGame == null)
                {
                    _gameRepository.AddOrUpdate(game);
                    returnGames.Add(game);
                }
            }
            catch
            {
                throw new Exception("Current game couldn't be saved to the db!" + game.Name);
            }
        }
        return View(returnGames);
    }

    public IActionResult LoadGameInfo()
    {
        List<Game> gamesList = _gameRepository.GetAll().ToList();
        
        if (gamesList.Count < 1)
        {
            ViewBag.MyString = "The library is empty!";
            return View();
        }
        else
        {
            List<GameVM> gameVMs = new List<GameVM>();
            string tempGenreString;
            foreach(var game in gamesList)
            {
                tempGenreString = "";
                var currentGame = _gameRepository.GetGameByAppId(game.AppId);
                if (currentGame.Genres == null)
                {
                    GameVM gameVM = _steamService.GetGameInfo(game);
                    //This seems to only happen with a single game I've tested - COD: MW3 -Multiplayer, but could happen for more.
                    if(gameVM._poco.response.data == null)
                    {
                        game.Genres = "Not Available";
                    }
                    else
                    {
                        gameVM._game = game;
                        gameVM._appId = game.AppId;
                        var genres = gameVM._poco.response.data.genres;
                        foreach(var genre in genres)
                        {
                            tempGenreString += genre.description + ",";
                        }
                        currentGame.Genres = tempGenreString.Substring(0, (tempGenreString.Length - 1));
                    }
                    _gameRepository.AddOrUpdate(currentGame);
                    gameVMs.Add(gameVM);
                }
            }
            return View(gameVMs);
        }
    }

    public IActionResult ViewGames()
    {
        throw new NotImplementedException();
    }

    public IActionResult ViewBannedIds()
    {
        return View(_blackListRepository.GetBlackList());
    }
}