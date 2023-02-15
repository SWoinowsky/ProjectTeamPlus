using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SteamProject.Models;

public partial class Game
{
    public int Id { get; set; }

    public string OwnerId { get; set; }

    public int AppId { get; set; }

    public string Name { get; set; } = null!;

    public int PlayTime { get; set; }

    public string IconUrl { get; set; } = null!;

    public int LastPlayed { get; set; }

    public bool Hidden { get; set; }

    public virtual User Owner { get; set; } = null!;

    public void FromJson(string obj, string userId)
    {
        JObject userGames = JObject.Parse(obj);
        AppId = (int) userGames["appid"];
        Name = (string) userGames["name"];
        PlayTime = (int) userGames["playtime_forever"];
        IconUrl = (string) userGames["img_icon_url"];
        LastPlayed = (int) userGames["rtime_last_played"];
        OwnerId = userId;
    }
}
