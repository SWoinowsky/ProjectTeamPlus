using System.Collections.Generic;
using SteamProject.Models;
using Microsoft.AspNetCore.Identity;

namespace SteamProject.ViewModels
{
    public class AdminUsersVM
    {
        public IEnumerable<User> steamUsers {get; set;}
        public IEnumerable<IdentityUser> identityUsers {get; set;}
    }
}