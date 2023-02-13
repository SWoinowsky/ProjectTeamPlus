using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class User
{
    public int Id { get; set; }

    public string AspNetUserId { get; set; } = null!;

    public string? SteamId { get; set; }

    public string? SteamName { get; set; }

    public string? ProfileUrl { get; set; }

    public string? AvatarUrl { get; set; }

    public int? PersonaState { get; set; }

    public int? PlayerLevel { get; set; }

    public virtual ICollection<Friend> Friends { get; } = new List<Friend>();

    public virtual ICollection<Game> Games { get; } = new List<Game>();

    public virtual ICollection<UserAchievement> UserAchievements { get; } = new List<UserAchievement>();
}
