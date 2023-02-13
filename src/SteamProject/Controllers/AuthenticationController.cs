/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OpenId.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamProject.Data;
using SteamProject.Extensions;
using SteamProject.Models;

namespace SteamProject.Controllers;

public class AuthenticationController : Controller
{
    private readonly SteamInfoDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthenticationController(SteamInfoDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public IActionResult Index()
    {
        return View();
    }


    public async Task<IActionResult> Link(int? steamId)
    {
        string id = _userManager.GetUserId((User));
        IdentityUser user = await _userManager.GetUserAsync(User);
        

        User fu = null;
        if (id != null)
        {
            fu = _context.Users.Where(u => u.AspNetUserId == id).FirstOrDefault();
            if (fu != null)
            {
                fu.SteamId = steamId;

                try
                {
                    _context.Update(fu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (fu.AspNetUserId == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
        }

      

        return View();
    }

    [HttpGet("~/signin")]
    public async Task<IActionResult> SignIn() => View("SignIn", await HttpContext.GetExternalProvidersAsync());

    [HttpPost("~/signin")]
    public async Task<IActionResult> SignIn([FromForm] string provider)
    {
        // Note: the "provider" parameter corresponds to the external
        // authentication provider choosen by the user agent.
        if (string.IsNullOrWhiteSpace(provider))
        {
            return BadRequest();
        }

        if (!await HttpContext.IsProviderSupportedAsync(provider))
        {
            return BadRequest();
        }

        // Instruct the middleware corresponding to the requested external identity
        // provider to redirect the user agent to its own authorization endpoint.
        // Note: the authenticationScheme parameter must match the value configured in Startup.cs
        var result = Challenge(new AuthenticationProperties { RedirectUri = "/" }, provider);
        return result;
    }

    [HttpGet("~/signout"), HttpPost("~/signout")]
    public IActionResult SignOutCurrentUser()
    {
        // Instruct the cookies middleware to delete the local cookie created
        // when the user agent is redirected from the external identity provider
        // after a successful authentication flow (e.g Google or Facebook).
        return SignOut(new AuthenticationProperties { RedirectUri = "/" },
            CookieAuthenticationDefaults.AuthenticationScheme);
    }
}