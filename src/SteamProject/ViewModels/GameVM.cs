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
    public class GameVM
    {
        public int _appId {get; set;}
        public Game _game {get; set;}
         public GameInfoPOCO _poco {get; set;}
        public GameVM()
        {
        }
    }
}