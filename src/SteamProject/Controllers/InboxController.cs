using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.Helpers;
using SteamProject.Models.DTO;
using SteamProject.Services;
using SteamProject.ViewModels;

namespace SteamProject.Controllers;

public class InboxController : Controller
{

    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;

    public InboxController(UserManager<IdentityUser> userManager, IUserRepository userRepository) 
    {
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public IActionResult Index()
    {
        string? id = _userManager.GetUserId(User);
        if (id is null)
        {
            return View();
        }
        User user = _userRepository.GetUser(id);
        List<InboxMessage> userMessages = user.InboxMessages.ToList();
        Console.WriteLine(user.InboxMessages.Count);
        return View(userMessages);
    }
}