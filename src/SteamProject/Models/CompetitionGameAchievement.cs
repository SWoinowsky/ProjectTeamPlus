using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class CompetitionGameAchievement
{
    public int Id { get; set; }

    public int CompetitionId { get; set; }

    public int GameAchievementId { get; set; }

    public virtual Competition Competition { get; set; } = null!;
}
