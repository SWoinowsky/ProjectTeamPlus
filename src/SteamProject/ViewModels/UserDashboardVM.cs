using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SteamProject.Models;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;
using SteamProject.Models.DTO;

namespace SteamProject.ViewModels
{
    public class UserDashboardVM
    {
        public User _user {get; set;}

        public List<List<Game>> RecentGames { get; set; }

        public List<String> RecentGamesNewsItems { get; set; }

        public List<List<Game>> FollowedGames { get; set; }

        public List<String> FollowedGamesNewsItems { get; set; }

        public UserDashboardVM()
        {

        }
    }
}