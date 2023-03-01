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
        public Game _game {get; set;}
        public int _appId {get; set;}

        public GameVM(Game game, int id)
        {
            _game = game;
            _appId = id;
        }
    }
}