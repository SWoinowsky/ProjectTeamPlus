using SteamProject.Models.DTO;
using System;
using System.Collections.Generic;

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

    public string? Genres { get; set; }

    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();

    public virtual ICollection<GameVote> GameVotes { get; set; } = new List<GameVote>();

    public virtual ICollection<UserGameInfo> UserGameInfos { get; set; } = new List<UserGameInfo>();

    public Game TakeLibraryInfoPOCO(Games game, int userId)
    {
        var temp = new Game();
        temp.AppId = game.appid;
        temp.Name = game.name;
        temp.IconUrl = game.img_icon_url;
        return temp;
    }
}
