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
    private readonly IIGDBGenresRepository _iGDBGenreRepository;


    public AdminController(SignInManager<IdentityUser> signInManager, IGameRepository gameRepository, UserManager<IdentityUser> userManager, IUserRepository userRepository, IBlackListRepository blackListRepository, IUserGameInfoRepository userGameInfoRepository, IFriendRepository friendRepository, ISteamService steamService, IIGDBGenresRepository iGDBGenresRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _blackListRepository = blackListRepository;
        _friendRepository = friendRepository;
        _steamService = steamService;
        _userRepository = userRepository;
        _userGameInfoRepository = userGameInfoRepository;
        _gameRepository = gameRepository;
        _iGDBGenreRepository = iGDBGenresRepository;
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

    public async Task<IActionResult> LoadGameInfoAsync()
    {
        HashSet<string> genreList = new HashSet<string>();
        List<Game> gamesList = await _gameRepository.GetAll().ToListAsync();
        
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
                if (currentGame.Genres == null || currentGame.Genres == "The genres couldn't be grabbed")
                {
                    try
                    {
                        var genreResults = await _steamService.GetGameInfoAsync(game.Name);
                        foreach(var genre in genreResults)
                        {
                            tempGenreString += genre + ",";
                            genreList.Add(genre);
                        }
                        currentGame.Genres = tempGenreString.Substring(0, (tempGenreString.Length - 1));
                    }
                    catch
                    {
                        tempGenreString = "The genres couldn't be grabbed";
                    }
                    var gameVM = _steamService.GetGameInfo(game);
                    
                    gameVM._game = game;
                    gameVM._appId = game.AppId;
                    if(currentGame.Genres == null)
                    {
                        currentGame.Genres = "The genres couldn't be grabbed";
                    }
                    _gameRepository.AddOrUpdate(currentGame);
                    gameVMs.Add(gameVM);
                }
            }

            var genres = _iGDBGenreRepository.GetGenreList();

            foreach(var genre in genreList)
            {
                bool contains = _iGDBGenreRepository.GetGenreList().Contains(genre);
                if(!contains)
                {
                    _iGDBGenreRepository.AddOrUpdate(new Igdbgenre {
                    Name = genre
                    });
                }
            }
            return View(gameVMs);
        }
    }

    public IActionResult ViewGames()
    {
        var gameList = _gameRepository.GetAll().ToList();
        return View(gameList);
    }

    public IActionResult ViewBannedIds()
    {
        return View(_blackListRepository.GetBlackList());
    }
}