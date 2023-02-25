using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class UserAchievement
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public int AchievementId { get; set; }

    public bool Achieved { get; set; }

    public DateTime? UnlockTime { get; set; }

    public virtual GameAchievement Achievement { get; set; } = null!;

    public virtual User Owner { get; set; } = null!;
}
