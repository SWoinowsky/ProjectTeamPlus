using System;
using System.Collections.Generic;

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
}
