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
        public User User {get; set;}

        public List<List<Game>> RecentGames { get; set; }

        public List<string[]> GamesNewsItems { get; set; }

        public List<List<Game>> FollowedGames { get; set; }

        public List<Competition> CurrentCompetitions { get; set; }

        public List<List<Competition>> PreviousCompetitions { get; set; }

        public UserDashboardVM()
        {

        }
    }
}