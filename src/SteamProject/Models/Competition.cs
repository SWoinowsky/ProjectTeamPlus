using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class Competition
{
    public int Id { get; set; }

    public int CreatorId { get; set; }

    public int GameId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual ICollection<CompetitionGameAchievement> CompetitionGameAchievements { get; } = new List<CompetitionGameAchievement>();

    public virtual ICollection<CompetitionPlayer> CompetitionPlayers { get; } = new List<CompetitionPlayer>();

    public virtual User Creator { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;
}
