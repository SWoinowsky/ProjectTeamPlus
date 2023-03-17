using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class CompetitionPlayer
{
    public int CompetitionId { get; set; }

    public string SteamId { get; set; } = null!;
}
