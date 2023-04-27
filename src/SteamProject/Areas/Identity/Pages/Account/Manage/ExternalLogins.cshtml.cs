// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Data;
using SteamProject.Helpers;
using SteamProject.Models;
using SteamProject.Models.Awards.Concrete;

namespace SteamProject.Areas.Identity.Pages.Account.Manage
{
    public class ExternalLoginsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserRepository _userRepo;
        private readonly IUserGameInfoRepository _userGameInfoRepo;
        private readonly IFriendRepository _friendRepo;
        private readonly IBlackListRepository _blackListRepository;
        private readonly IBadgeRepository _badgeRepository;
        private readonly IUserBadgeRepository _userBadgeRepository;


        public ExternalLoginsModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUserStore<IdentityUser> userStore, 
            IUserRepository userRepo, 
            IUserGameInfoRepository userGameInfoRepo, 
            IFriendRepository friendRepo, 
            IBlackListRepository blackListRepository, 
            IBadgeRepository badgeRepository, 
            IUserBadgeRepository userBadgeRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
            _userRepo = userRepo;
            _userGameInfoRepo = userGameInfoRepo;
            _friendRepo = friendRepo;
            _blackListRepository = blackListRepository;
            _badgeRepository = badgeRepository;
            _userBadgeRepository = userBadgeRepository;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> OtherLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool ShowRemoveButton { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            CurrentLogins = await _userManager.GetLoginsAsync(user);
            OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();

            string passwordHash = null;
            if (_userStore is IUserPasswordStore<IdentityUser> userPasswordStore)
            {
                passwordHash = await userPasswordStore.GetPasswordHashAsync(user, HttpContext.RequestAborted);
            }

            ShowRemoveButton = passwordHash != null || CurrentLogins.Count > 1;
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            if (!result.Succeeded)
            {
                StatusMessage = "The external login was not removed.";
                return RedirectToPage();
            }

            User currentUser = null;
            if (user != null)
            {

                if (user.Id != null)
                {
                    currentUser = _userRepo.GetAll().FirstOrDefault(u => u.AspNetUserId == user.Id);
                    var currentUserGameInfo = _userGameInfoRepo.GetAll().Where(g => g.OwnerId == currentUser.Id).ToList();
                    var friendInfo = _friendRepo.GetAll().Where(f => f.RootId == currentUser.Id).ToList();

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


                            for (int i = 0; i < currentUserGameInfo.Count; i++)
                            {
                                _userGameInfoRepo.Delete(currentUserGameInfo[i]);
                            }

                            for (int i = 0; i < friendInfo.Count(); i++)
                            {
                                _friendRepo.Delete(friendInfo[i]);
                            }
                            

                            _userRepo.AddOrUpdate(currentUser);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            throw;
                        }
                    }
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "The external login was removed.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var info = await _signInManager.GetExternalLoginInfoAsync(userId);
            if (info == null)
            {
                throw new InvalidOperationException($"Unexpected error occurred loading external login info.");
            }

            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                StatusMessage = "The external login was not added. External logins can only be associated with one account.";
                return RedirectToPage();
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            if (info.ProviderKey != null)
            {
                string[] urlSplit = info.ProviderKey.Split('/');
                string steamId = urlSplit.Last();

                // This is where we get the Steam Id and check the blacklist for it.
                if(_blackListRepository.CheckForBlackList(steamId))
                {
                    StatusMessage = "That id has been banned for cheating!";
                    info.ProviderKey = "";
                    info.ProviderDisplayName = "";
                    return RedirectToPage();
                }

                User currentUser = null;
                if (userId != null)
                {
                    //Store steamid in database here somehow
                    if (steamId != null)
                    {
                        currentUser = _userRepo.GetAll().Where(u => u.AspNetUserId == userId).FirstOrDefault();
                        if(currentUser != null) {
                            try
                            {
                                currentUser.SteamId = steamId;
                                
                                _userRepo.AddOrUpdate(currentUser);

                            }
                            catch (DbUpdateConcurrencyException)
                            {
                                throw;
                            }

                            return Redirect("/Library/Index");
                        }
                    }
                }
            }

            StatusMessage = "The external login was added.";
            return RedirectToPage();
        }
    }
}
