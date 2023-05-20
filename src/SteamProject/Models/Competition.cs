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

    public int StatusId { get; set; }

    public string? Goal { get; set; }

    public virtual ICollection<CompetitionGameAchievement> CompetitionGameAchievements { get; set; } = new List<CompetitionGameAchievement>();

    public virtual ICollection<CompetitionPlayer> CompetitionPlayers { get; set; } = new List<CompetitionPlayer>();

    public virtual ICollection<CompetitionVote> CompetitionVotes { get; set; } = new List<CompetitionVote>();

    public virtual User Creator { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;

    public virtual ICollection<SpeedRun> SpeedRuns { get; } = new List<SpeedRun>();

    public virtual Status Status { get; set; } = null!;
}
