using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SteamProject.Models;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SteamProject.ViewModels
{
    public class UserDashboardVM
    {
        public User _user {get; set;}

        public List<List<Game>> recentGames { get; set; }

        public List<List<Game>> followedGames { get; set; }

        public UserDashboardVM()
        {

        }
    }
}