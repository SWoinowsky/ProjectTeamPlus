using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class UserAchievement
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public int GameId { get; set; }

    public string ApiName { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public bool Achieved { get; set; }

    public int? UnlockTime { get; set; }

    public virtual User Owner { get; set; } = null!;
    
}
