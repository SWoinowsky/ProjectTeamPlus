using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class GameAchievement
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public string ApiName { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string IconAchievedUrl { get; set; } = null!;

    public string IconHiddenUrl { get; set; } = null!;

    public bool HiddenFromUsers { get; set; }

    public virtual ICollection<UserAchievement> UserAchievements { get; } = new List<UserAchievement>();
}
