using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class Competition
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual Game Game { get; set; } = null!;
}
