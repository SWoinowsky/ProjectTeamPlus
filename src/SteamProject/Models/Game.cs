using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SteamProject.Models.DTO;

namespace SteamProject.Models;

public partial class Game
{
    public int Id { get; set; }

    public int AppId { get; set; }

    public string Name { get; set; } = null!;

    public string? DescShort { get; set; }

    public string? DescLong { get; set; }

    public int? PlayTime { get; set; }

    public string? IconUrl { get; set; }

    public int? LastPlayed { get; set; }

    public virtual ICollection<Competition> Competitions { get; } = new List<Competition>();

    public virtual ICollection<UserGameInfo> UserGameInfos { get; } = new List<UserGameInfo>();

    public void FromJson(string obj, int userId, User user)
    {
        JObject userGames = JObject.Parse(obj);
        AppId = (int) userGames["appid"];
        Name = (string) userGames["name"];
        PlayTime = (int) userGames["playtime_forever"];
        IconUrl = (string) userGames["img_icon_url"];
        LastPlayed = (int) userGames["rtime_last_played"];
    }
    
    public Game TakeLibraryInfoPOCO(Games game, int userId)
    {
        var temp = new Game();
        temp.AppId = game.appid;
        temp.Name = game.name;
        temp.PlayTime = game.playtime_forever;
        temp.LastPlayed = game.rtime_last_played;
        temp.IconUrl = game.img_icon_url;
        temp.DescLong = "";
        temp.DescShort = "";
        return temp;
    }
}
