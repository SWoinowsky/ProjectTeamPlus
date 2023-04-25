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

    public AdminController(
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager, IUserRepository userRepository, IBlackListRepository blackListRepository, IUserGameInfoRepository userGameInfoRepository, IFriendRepository friendRepository, ISteamService steamService)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _signInManager = signInManager;
        _blackListRepository = blackListRepository;
        _friendRepository = friendRepository;
        _steamService = steamService;
        _userGameInfoRepository = userGameInfoRepository;
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
        var temp = _steamService.GetSteamCuratorGames();
        throw new NotImplementedException();
    }

    public IActionResult LoadGameInfo()
    {
        throw new NotImplementedException();
    }

    public IActionResult ViewGames()
    {
        throw new NotImplementedException();
    }

    public IActionResult ViewBannedIds()
    {
        return View(_blackListRepository.GetBlackList());
    }
}