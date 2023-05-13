using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SteamProject.Models;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;
using SteamProject.Models.DTO;
using Newtonsoft.Json.Linq;

namespace SteamProject.ViewModels
{
    public class RecommendationVM
    {
        public User _user {get; set;}
        public List<Dictionary<Game, int>> scoredGames {get; set;}
    }
}