using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class Team
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Captain { get; set; }

    public string Motto { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public int? CombinedScore { get; set; }
}
