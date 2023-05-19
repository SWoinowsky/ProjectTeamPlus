using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Models;

namespace SteamProject.ViewComponents
{
    public class MessagesViewComponent : ViewComponent
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public MessagesViewComponent(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            string? id = _userManager.GetUserId((ClaimsPrincipal)User);
            string messengerId;
            if (id is null)
            {
                Console.WriteLine("Id is null");
                messengerId = "null";
            }
            else
            {

                var user = _userRepository.GetUser(id);
                if (user != null)
                {
                    messengerId = user.Id.ToString();
                }
                messengerId = "null";


            }
            return View("_Messages", messengerId);
        }
    }
}