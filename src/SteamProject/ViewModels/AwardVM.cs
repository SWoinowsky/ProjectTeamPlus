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
    public class AwardVM
    {
        public string AwardTitle { get; set; }
        public string AwardMessage { get; set; }
        public string AwardImageBase64 { get; set; }

        public AwardVM()
        {

        }
    }
}