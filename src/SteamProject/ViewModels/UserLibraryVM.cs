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
    public class UserLibraryVM
    {
        public User _user {get; set;}
        public HashSet<Game> _games {get; set;}
        public HashSet<string> _genres {get; set;}

        public UserLibraryVM()
        {

        }
    }
}