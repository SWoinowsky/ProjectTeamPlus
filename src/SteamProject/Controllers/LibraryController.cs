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

public class LibraryController: Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly ISteamService _steamService;
    private readonly IUserGameInfoRepository _userGameInfoRepository;
    private readonly IIGDBGenresRepository _iGDBGenreRepository;

    public LibraryController(UserManager<IdentityUser> userManager, IUserRepository userRepository, IGameRepository gameRepository, IUserGameInfoRepository userGameInfoRepository, ISteamService steamService, IIGDBGenresRepository iGDBGenresRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _steamService = steamService;
        _userGameInfoRepository = userGameInfoRepository;
        _iGDBGenreRepository = iGDBGenresRepository;
    }

    [Authorize]
    public async Task<IActionResult> Index(bool refresh)
    {
        // Going to be used to populate the genre table
        HashSet<string> genreList = new HashSet<string>();
        string? id = _userManager.GetUserId(User);

        if (refresh == null)
        {
            refresh = false;
        }

        var userLibraryVM = new UserLibraryVM();

        if(id is null)
        {
            return View();
        }
        else
        {
            User user = _userRepository.GetUser(id);

            if (user.AvatarUrl == null && user.SteamId != null)
            {
                var steamUser = _steamService.GetSteamUser(user.SteamId);
                user.SteamName = steamUser.SteamName;
                user.AvatarUrl = steamUser.AvatarUrl;
                var SteamLevel = _steamService.GetUserLevel(user.SteamId);

                user.PlayerLevel = SteamLevel;

                _userRepository.AddOrUpdate(user);
            }

            userLibraryVM._user = user;
            List<UserGameInfo> gameInfo = _userGameInfoRepository.GetAllUserGameInfo(user.Id);
            userLibraryVM._games = new HashSet<Game>();
            UserGameInfo? currentUserInfo = new UserGameInfo();

            if (gameInfo.Count == 0)
            {
                refresh = true;
            }
            if (user.SteamId == null)
            {
                return View();
            }
            if(refresh == true)
            {
                List<Game>? games = _steamService.GetGames(user.SteamId, user.Id).ToList();
                
                if(games.Count == 0)
                    return View();

                // Checks to see  if each individual game from Steam is in our db or not on a library refresh
                // --- Refresh isn't the page refreshing, it's the user initiating a refresh to get newly added games
                foreach(var game in games)
                {
                    try
                    {
                        var currentGame = _gameRepository.GetGameByAppId(game.AppId);
                        var tempGenreString = "";
                        if(currentGame != null && currentGame.Genres == null)
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
                            
                            if(currentGame.Genres == null)
                            {
                                currentGame.Genres = "The genres couldn't be grabbed";
                            }
                        }

                        int? playTime = game.PlayTime;
                        int? lastPlayed = game.LastPlayed;

                        game.PlayTime = 0;
                        game.LastPlayed = 0;

                        try
                        {
                            currentUserInfo = _userGameInfoRepository.GetUserInfoForGame( game.AppId, user.Id );
                        }
                        catch
                        {
                            currentUserInfo = null;
                        }

                        //Check if game is in database, if not add it
                        if (currentGame == null)
                        {
                            if(refresh)
                            {
                                try
                                {
                                    var genreResults = await _steamService.GetGameInfoAsync(game.Name);
                                    foreach(var genre in genreResults)
                                    {
                                        tempGenreString += genre + ",";
                                        genreList.Add(genre);
                                    }
                                    game.Genres = tempGenreString.Substring(0, (tempGenreString.Length - 1));
                                }
                                catch
                                {
                                    tempGenreString = "The genres couldn't be grabbed";
                                }
                                
                                if(game.Genres == null)
                                {
                                    game.Genres = "The genres couldn't be grabbed";
                                }
                                _gameRepository.AddOrUpdate(game);
                            }

                            var temp = _gameRepository.GetAll(g => g.AppId == game.AppId).FirstOrDefault();
                            if (currentUserInfo == null)
                            {
                                _userGameInfoRepository.AddOrUpdate(new UserGameInfo{
                                    OwnerId = user.Id,
                                    GameId = temp.Id,
                                    PlayTime = playTime,
                                    LastPlayed = lastPlayed,
                                    Hidden = false,
                                    Followed = false,
                                    Game = game,
                                    Owner = user
                                });
                                userLibraryVM._games.Add(game);
                            }
                            else
                            {
                                UserGameInfo currentGameInfo = gameInfo.Single(g => g.GameId == temp.Id);
                                currentGameInfo.LastPlayed = lastPlayed;
                                currentGameInfo.PlayTime = playTime;
                                _userGameInfoRepository.AddOrUpdate(currentGameInfo);
                                userLibraryVM._games.Add(game);
                            }
                        }
                        else
                        {
                            if (currentUserInfo == null)
                            {

                                UserGameInfo newInfo = new UserGameInfo
                                {
                                    OwnerId = user.Id,
                                    GameId = currentGame.Id,
                                    PlayTime = playTime,
                                    LastPlayed = lastPlayed,
                                    Hidden = false,
                                    Followed = false,
                                    Owner = user,
                                    Game = currentGame
                                };
                                _userGameInfoRepository.AddOrUpdate(newInfo);
                                userLibraryVM._games.Add(game);

                            }
                            else
                            {
                                UserGameInfo currentGameInfo = gameInfo.Single(g => g.GameId == currentGame.Id);
                                currentGameInfo.LastPlayed = lastPlayed;
                                currentGameInfo.PlayTime = playTime;
                                _userGameInfoRepository.AddOrUpdate(currentGameInfo);
                                userLibraryVM._games.Add(game);
                            }
                        }
                    }
                    catch
                    {
                        throw new Exception("Current game couldn't be saved to the db!" + game.Name);
                    }
                }
            }
            else
            {
                userLibraryVM._games = _gameRepository.GetGamesListByUserInfo(gameInfo);
            }
            var genres = _iGDBGenreRepository.GetGenreList().OrderBy(s => s);
            userLibraryVM._genres = new HashSet<string>();

            if(genreList.Count() == 0)
            {
                foreach(var genre in genres)
                    userLibraryVM._genres.Add(genre);
            }
            else
            {
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
            }
            userLibraryVM._user.UserGameInfos = userLibraryVM._user.UserGameInfos.OrderBy(g => g.Game.Name).ToList();
            return View(userLibraryVM);
        }
    }
    public IActionResult ShowMoreInfo(int appId)
    {
        string? id = _userManager.GetUserId(User);
        User user = _userRepository.GetUser(id);

        Game game = _gameRepository.GetGameByAppId(appId);
        GameVM gameVM = _steamService.GetGameInfo(game);

        gameVM._game = game;
        gameVM._appId = appId;
        gameVM._userGame = _userGameInfoRepository.GetAll(g => g.GameId == game.Id).FirstOrDefault();
        
        gameVM.playTime = Math.Round(Convert.ToDouble(gameVM._userGame.PlayTime)/60, 1);

        gameVM.cleanRequirements();
        gameVM.cleanDescriptions();

        return View(gameVM);
    }

    [HttpGet]
    public IActionResult Sort(string genre)
    {
        ViewBag.MyString = genre;
        string? id = _userManager.GetUserId(User);
        User user = _userRepository.GetUser(id);
        List<UserGameInfo> gameInfo = _userGameInfoRepository.GetAllUserGameInfo(user.Id);

        var userLibraryVM = new UserLibraryVM();
        userLibraryVM._games = new HashSet<Game>();
        userLibraryVM._user = user;
        UserGameInfo? currentUserInfo = new UserGameInfo();

        // Grab the list of games a user has
        List<Game>? games = _steamService.GetGames(user.SteamId, user.Id).ToList();

        HashSet<UserGameInfo> userGamesByGenre = new HashSet<UserGameInfo>();

        if(games.Count == 0)
            return View();

        foreach(var game in games)
        {
            try
            {
                var currentGame = _gameRepository.GetGameByAppId(game.AppId);
                foreach(var currentGenre in currentGame.Genres.Split(",").ToList())
                {
                    if(currentGenre == genre)
                    {
                        currentUserInfo = _userGameInfoRepository.GetUserInfoForGame(game.AppId, user.Id);
                        userGamesByGenre.Add(currentUserInfo);
                        userLibraryVM._games.Add(game);
                    }
                }
            }
            catch
            {
                // Don't need it to do anything here since we are just getting info.
            }
        }
        userLibraryVM._userGameInfo = userGamesByGenre;
        return View(userLibraryVM);
    }
}