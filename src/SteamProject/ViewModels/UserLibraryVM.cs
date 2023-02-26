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
        public List<Game> _games {get; set;}

        public UserLibraryVM()
        {

        }
    }
}