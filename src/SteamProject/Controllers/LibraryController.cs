using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.Data;
using SteamProject.Models;
using Microsoft.AspNetCore.Identity;

namespace SteamProject.Controllers;

public class LibraryController: Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepostory;
    private readonly IGameRepository _gameRepository;

    public LibraryController(UserManager<IdentityUser> userManager, IUserRepository userRepository, IGameRepository gameRepository)
    {
        _userManager = userManager;
        _userRepostory = userRepository;
        _gameRepository = gameRepository;
    }

    public IActionResult Index()
    {
        var id = _userManager.GetUserId(User);
        var user = _userRepostory.GetUser(id);
        //user.Games.Add();
        return View(user);
    }

}