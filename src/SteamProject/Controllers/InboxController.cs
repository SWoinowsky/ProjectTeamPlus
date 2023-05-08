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
    private readonly IInboxRepository _inboxRepository;


    public InboxController(UserManager<IdentityUser> userManager, IUserRepository userRepository, IInboxRepository inboxRepository) 
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _inboxRepository = inboxRepository;
    }

    public IActionResult Index()
    {
        string? id = _userManager.GetUserId(User);
        if (id is null)
        {
            return View();
        }
        User user = _userRepository.GetUser(id);
        List<InboxMessage> userMessages = _inboxRepository.GetInboxMessages(user.Id).OrderByDescending(s => s.Id).ToList();
        return View(userMessages);
    }
}