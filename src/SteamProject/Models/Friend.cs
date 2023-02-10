using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class Friend
{
    public int Id { get; set; }

    public int RootId { get; set; }

    public int SteamId { get; set; }

    public string SteamName { get; set; } = null!;

    public int PersonaState { get; set; }

    public string AvatarUrl { get; set; } = null!;

    public int LastLogOff { get; set; }

    public string? GameExtraInfo { get; set; }

    public int? GameId { get; set; }

    public virtual User Root { get; set; } = null!;
}
