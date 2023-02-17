using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SteamProject.Models.DTO;

namespace SteamProject.Models;

public partial class Game
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public int AppId { get; set; }

    public string Name { get; set; } = null!;

    public string DescShort { get; set; } = null!;

    public string DescLong { get; set; } = null!;

    public int PlayTime { get; set; }

    public string IconUrl { get; set; } = null!;

    public int LastPlayed { get; set; }

    public bool Hidden { get; set; }

    public virtual User Owner { get; set; } = null!;

    public void FromJson(string obj, int userId, User user)
    {
        JObject userGames = JObject.Parse(obj);
        AppId = (int) userGames["appid"];
        Name = (string) userGames["name"];
        PlayTime = (int) userGames["playtime_forever"];
        IconUrl = (string) userGames["img_icon_url"];
        LastPlayed = (int) userGames["rtime_last_played"];
        OwnerId = userId;
        Owner = user;
    }

    public void TakeGameInfoPOCO(GameInfoPOCO poco)
    {
        if(poco.response.data != null)
        {
            var userData = poco.response.data;
            if(userData.detailed_description == null) 
                DescLong = "No description available";
            else
                DescLong = userData.detailed_description;
            if(userData.short_description == null)
                DescShort = "No description available";
            else
                DescShort = userData.short_description;
        }
        else
        {
            DescLong = "No description available";
            DescShort = "No description available";
        }
    }

}
