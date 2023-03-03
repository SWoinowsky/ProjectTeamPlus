using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class GameAchievement
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public string ApiName { get; set; } = null!;

    public string? DisplayName { get; set; }

    public string? IconAchievedUrl { get; set; }

    public string? IconHiddenUrl { get; set; }

    public bool HiddenFromUsers { get; set; }

    public virtual ICollection<UserAchievement> UserAchievements { get; } = new List<UserAchievement>();
}
