using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class GameVote
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public int UserId { get; set; }

    public bool Vote { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
