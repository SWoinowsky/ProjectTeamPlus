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
        public void cleanRequirements()
        {
            this._poco.response.data.linux_requirements.minimum = Regex.Replace(this._poco.response.data.linux_requirements.minimum, @"<[^>]+>|&nbsp;", "").Trim();
            this._poco.response.data.mac_requirements.minimum = Regex.Replace(this._poco.response.data.mac_requirements.minimum, @"<[^>]+>|&nbsp;", "").Trim();
            this._poco.response.data.pc_requirements.minimum = Regex.Replace(this._poco.response.data.pc_requirements.minimum, @"<[^>]+>|&nbsp;", "").Trim();
            this._poco.response.data.pc_requirements.recommended = Regex.Replace(this._poco.response.data.pc_requirements.recommended, @"<[^>]+>|&nbsp;", "").Trim();
            this._poco.response.data.linux_requirements.recommended = Regex.Replace(this._poco.response.data.linux_requirements.recommended, @"<[^>]+>|&nbsp;", "").Trim();
            this._poco.response.data.mac_requirements.recommended = Regex.Replace(this._poco.response.data.mac_requirements.recommended, @"<[^>]+>|&nbsp;", "").Trim();
        }
    }

    
}