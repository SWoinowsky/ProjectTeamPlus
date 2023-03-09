using System;
using System.Collections.Generic;
using SteamProject.DAL.Abstract;

namespace SteamProject.Models;

public partial class UserGameInfo
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public int GameId { get; set; }

    public int? PlayTime { get; set; }

    public int? LastPlayed { get; set; }

    public bool Hidden { get; set; }

    public bool Followed { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual User Owner { get; set; } = null!;

    public UserGameInfo GetGameByIdAndUser (int id, IUserGameInfoRepository userGameInfoRepository, int userId)
    {
       UserGameInfo returnGame = userGameInfoRepository.GetAll(g => g.Game.AppId == id).Where( g => g.OwnerId == userId ).FirstOrDefault();
       return returnGame;
    }
}
