using System;
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

namespace SteamProject.Controllers;

public class LibraryController: Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly ISteamService _steamService;
    private readonly IUserGameInfoRepository _userGameInfoRepository;

    public LibraryController(UserManager<IdentityUser> userManager, IUserRepository userRepository, IGameRepository gameRepository,IUserGameInfoRepository userGameInfoRepository, ISteamService steamService)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _steamService = steamService;
        _userGameInfoRepository = userGameInfoRepository;
    }

    [Authorize]
    public IActionResult Index(bool refresh)
    {
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
            userLibraryVM._user = user;
            List<UserGameInfo> gameInfo = _userGameInfoRepository.GetAllUserGameInfo(user.Id);
            userLibraryVM._games = new List<Game>();

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

                foreach(var game in games)
                {
                    try
                    {
                        var currentGame = _gameRepository.GetGameByAppId(game.AppId);

                        int? playTime = game.PlayTime;
                        int? lastPlayed = game.LastPlayed;

                        game.PlayTime = 0;
                        game.LastPlayed = 0;

                        UserGameInfo? currentUserInfo = _userGameInfoRepository.GetUserInfoForGame(game.Id, user.Id);

                        //Check if game is in database, if not add it
                        if (currentGame == null)
                        {
                            if (currentUserInfo == null)
                            {
                                _userGameInfoRepository.AddOrUpdate(new UserGameInfo{
                                    OwnerId = user.Id,
                                    GameId = game.Id,
                                    PlayTime = playTime,
                                    LastPlayed = lastPlayed,
                                    Hidden = false,
                                    Followed = false,
                                    Game = game,
                                    Owner = user
                                });
                                userLibraryVM._games.Add(game);
                            }
                            _gameRepository.AddOrUpdate(game);
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
                // var games = tempGameInfo;
                // if(games == null)
                //     return View();
                // foreach(var game in games)
                // {  
                //     var tempGame = _gameRepository.FindById(game.GameId);
                //     userLibraryVM._games.Add(tempGame);
                // }
                userLibraryVM._games = _gameRepository.GetGamesListByUserInfo(gameInfo);
            }
            return View(userLibraryVM);
        }
    }

    public IActionResult ShowMoreInfo(int appId)
    {
        var game = _gameRepository.GetAll(g => g.AppId == appId).FirstOrDefault();
        var gameVM = _steamService.GetGameInfo(game);
        gameVM._game = game;
        gameVM._appId = appId;
        return View(gameVM);
    }
}