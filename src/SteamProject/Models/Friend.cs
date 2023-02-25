using System;
using System.Collections.Generic;
using SteamProject.Models.DTO;

namespace SteamProject.Models;

public partial class Friend
{
    public int Id { get; set; }

    public int RootId { get; set; }

    public string SteamId { get; set; } = null!;

    public string SteamName { get; set; } = null!;

    public int PersonaState { get; set; }

    public string AvatarUrl { get; set; } = null!;

    public string AvatarFullUrl { get; set; } = null!;

    public int LastLogOff { get; set; }

    public string? GameExtraInfo { get; set; }

    public string? GameId { get; set; }

    public virtual User Root { get; set; } = null!;

    public void TakePlayerPOCO( Player userPOCOIn )
    {
        SteamId = userPOCOIn.steamid;
        SteamName = userPOCOIn.personaname;
        PersonaState = userPOCOIn.personastate;
        AvatarUrl = userPOCOIn.avatar;
        AvatarFullUrl = userPOCOIn.avatarfull;
        LastLogOff = userPOCOIn.lastlogoff;
        GameExtraInfo = userPOCOIn.gameextrainfo;
        GameId = userPOCOIn.gameid;
    }
}
