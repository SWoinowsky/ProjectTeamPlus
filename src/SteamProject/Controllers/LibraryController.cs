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

namespace SteamProject.Controllers;

public class LibraryController: Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly ISteamService _steamService;

    public LibraryController(UserManager<IdentityUser> userManager, IUserRepository userRepository, IGameRepository gameRepository, ISteamService steamService)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _steamService = steamService;
    }

    [Authorize]
    public IActionResult Index()
    {
        if(_userManager.GetUserId(User) is null)
        {
            return View();
        }
        else
        {
            var id = _userManager.GetUserId(User);
            var user = _userRepository.GetUser(id);
            var temp = _gameRepository.GetAll(g => g.OwnerId == user.Id).ToList();
            if(temp.Count() == 0)
            {
                var games = _steamService.GetGames(user.SteamId, user.Id);
                if(games == null)
                    return View();
                foreach(var game in games)
                {
                    try{
                        _gameRepository.AddOrUpdate(game);
                    }
                    catch
                    {
                        throw new Exception("Current game couldn't be saved to the db!" + game.Name);
                    }
                }
            }
            else
            {
                var games = temp;
                if(games == null)
                    return View();
                user.Games.Clear();
                foreach(var game in games)
                {  
                    user.Games.Add(game);
                }
            }
            return View(user);
        }
    }
}