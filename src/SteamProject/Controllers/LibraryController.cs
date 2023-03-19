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

                foreach(var game in games)
                {
                    try
                    {
                        var currentGame = _gameRepository.GetGameByAppId(game.AppId);

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
                            var context = new ValidationContext(game);
                            var results = new List<ValidationResult>();
                            var isValid = Validator.TryValidateObject(game, context, results);
                            if(isValid)
                                _gameRepository.AddOrUpdate(game);

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

                                
                                context = new ValidationContext(currentGameInfo);
                                results = new List<ValidationResult>();
                                isValid = Validator.TryValidateObject(currentGameInfo, context, results);
                                if(isValid)
                                    _userGameInfoRepository.AddOrUpdate(currentGameInfo);

                                context = new ValidationContext(game);
                                results = new List<ValidationResult>();
                                isValid = Validator.TryValidateObject(game, context, results);
                                if( isValid )
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

                                var context = new ValidationContext(newInfo);
                                var results = new List<ValidationResult>();
                                var isValid = Validator.TryValidateObject(newInfo, context, results);
                                if(isValid)
                                    _userGameInfoRepository.AddOrUpdate(newInfo);

                                context = new ValidationContext(game);
                                results = new List<ValidationResult>();
                                isValid = Validator.TryValidateObject(game, context, results);
                                if( isValid )
                                    userLibraryVM._games.Add(game);

                            }
                            else
                            {
                                UserGameInfo currentGameInfo = gameInfo.Single(g => g.GameId == currentGame.Id);
                                currentGameInfo.LastPlayed = lastPlayed;
                                currentGameInfo.PlayTime = playTime;

                                var context = new ValidationContext(currentGameInfo);
                                var results = new List<ValidationResult>();
                                var isValid = Validator.TryValidateObject(currentGameInfo, context, results);
                                if(isValid)
                                    _userGameInfoRepository.AddOrUpdate(currentGameInfo);

                                context = new ValidationContext(game);
                                results = new List<ValidationResult>();
                                isValid = Validator.TryValidateObject(game, context, results);
                                if( isValid )
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
}