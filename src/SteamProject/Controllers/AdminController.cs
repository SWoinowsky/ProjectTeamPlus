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

    public AdminController(UserManager<IdentityUser> userManager, IUserRepository userRepository, IBlackListRepository blackListRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _blackListRepository = blackListRepository;
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

    public IActionResult LoadGames()
    {
        throw new NotImplementedException();
    }

    public IActionResult ViewGames()
    {
        throw new NotImplementedException();
    }
}