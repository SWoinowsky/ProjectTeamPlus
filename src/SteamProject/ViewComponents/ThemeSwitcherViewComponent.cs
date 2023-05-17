using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Models;

namespace SteamProject.ViewComponents
{
    public class ThemeSwitcherViewComponent : ViewComponent
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public ThemeSwitcherViewComponent(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            string? id = _userManager.GetUserId((ClaimsPrincipal)User);
            string theme;
            if (id is null)
            {
                theme = "light"; // Set default theme if user not found
            }
            else
            {
                User user = _userRepository.GetUser(id);

                if (user != null)
                {
                    theme = user.Theme ?? "light"; // Default to light theme if not set
                }
                else
                {
                    theme = "light"; // Default to light theme if not set
                }

            }
            return View("_ThemeSwitcher", theme);
        }

    }
}