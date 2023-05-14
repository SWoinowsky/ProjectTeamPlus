using System;
using System.Collections.Generic;
using SteamProject.Models.DTO;

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

    public string? Theme { get; set; }

    public virtual ICollection<CompetitionVote> CompetitionVotes { get; set; } = new List<CompetitionVote>();

    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();

    public virtual ICollection<Friend> Friends { get; set; } = new List<Friend>();

    public virtual ICollection<GameVote> GameVotes { get; set; } = new List<GameVote>();

    public virtual ICollection<InboxMessage> InboxMessages { get; set; } = new List<InboxMessage>();

    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();

    public virtual ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();

    public virtual ICollection<UserGameInfo> UserGameInfos { get; set; } = new List<UserGameInfo>();

    public void TakeSteamPOCO(SteamUserPOCO poco)
    {
        var userData = poco.response.players[0];

        SteamId = userData.steamid;
        SteamName = userData.personaname;
        ProfileUrl = userData.profileurl;
        AvatarUrl = userData.avatarfull;
        PersonaState = userData.personastate;
    }

    public void AddSteamInfo(User user)
    {
        SteamId = user.SteamId;
        SteamName = user.SteamName;
        ProfileUrl = user.ProfileUrl;
        AvatarUrl = user.AvatarUrl;
        PersonaState = user.PersonaState;
    }
}
