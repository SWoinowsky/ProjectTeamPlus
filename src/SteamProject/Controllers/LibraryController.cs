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

    public LibraryController(UserManager<IdentityUser> userManager, IUserRepository userRepository)
    {
        _userManager = userManager;
        _userRepostory = userRepository;
    }

    public IActionResult Index()
    {
        var id = _userManager.GetUserId(User);
        var user = _userRepostory.GetUser(id);
        return View(user);
    }

}