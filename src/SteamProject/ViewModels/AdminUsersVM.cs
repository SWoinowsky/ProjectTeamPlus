using System.Collections.Generic;
using SteamProject.Models;
using Microsoft.AspNetCore.Identity;

namespace SteamProject.ViewModels
{
    public class AdminUsersVM
    {
        public string AspNetUserId {get; set;}
        public string SteamId {get; set;}
        public string SteamName {get; set;}
        public string Email {get; set;}
    }
}