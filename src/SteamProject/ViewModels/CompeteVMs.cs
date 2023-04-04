
using SteamProject.Models;
using SteamProject.Models.DTO;

namespace SteamProject.ViewModels;


public class CompeteIndexVM
{
    public List<Competition> Competitions { get; set; }

    public CompeteIndexVM(){}

}

public class CompeteDetailsVM
{
    public Competition CurrentComp { get; set; } = null;
    public Game Game { get; set; } = null;
    public List<CompetitionPlayer> CompPlayers { get; set; } = null;
    public List<User> Players { get; set; } = null;
}


public class CompeteInitiateVM
{
    public List<UserAchievement> UsersAchievements { get; set; } = null!;
    public List<UserAchievement> FriendsAchievements { get; set; } = null!;
    public Competition CurrentCompetition { get; set; } = null!;
    public Game ChosenGame { get; set; } = null!;
    public string MySteamId { get; set; }

    public CompeteInitiateVM(){}

    public CompeteInitiateVM( List<UserAchievement> users, List<UserAchievement> friends ) 
    {
        UsersAchievements = users;
        FriendsAchievements = friends;
    }
}