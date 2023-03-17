using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class CompetitionPlayer
{
    public int Id { get; set; }

    public int CompetitionId { get; set; }

    public string SteamId { get; set; } = null!;

    public virtual Competition Competition { get; set; } = null!;
}
