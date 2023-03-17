
using SteamProject.Models;

namespace SteamProject.ViewModels;


public class CompeteIndexVM
{
    public List<Competition> Competitions { get; set; }

    public CompeteIndexVM(){}

}


public class CompeteInitiateVM
{
    public List<UserAchievement> UsersAchievements { get; set; } = null!;
    public List<UserAchievement> FriendsAchievements { get; set; } = null!;
    public Competition CurrentCompetition { get; set; } = null!;
    public string MySteamId { get; set; }

    public CompeteInitiateVM(){}

    public CompeteInitiateVM( List<UserAchievement> users, List<UserAchievement> friends ) 
    {
        UsersAchievements = users;
        FriendsAchievements = friends;
    }
}