using System;
using System.Collections.Generic;
using SteamProject.Models.DTO;

namespace SteamProject.Models;

public partial class GameAchievement
{
    public int Id { get; set; }

    public int PointVal { get; set; }

    public int GameId { get; set; }

    public string ApiName { get; set; } = null!;

    public string? DisplayName { get; set; }

    public string? IconAchievedUrl { get; set; }

    public string? IconHiddenUrl { get; set; }

    public bool HiddenFromUsers { get; set; }

    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();

    public GameAchievement(){}

    public GameAchievement( SchemaAchievement achievementPOCO )
    {
        ApiName = achievementPOCO.name;
        DisplayName = achievementPOCO.displayName;
        IconAchievedUrl = achievementPOCO.icon;
        IconHiddenUrl = achievementPOCO.icongray;
        HiddenFromUsers = ( achievementPOCO.hidden == 1 );
    }
}
