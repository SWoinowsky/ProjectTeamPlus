
using SteamProject.Models;

namespace SteamProject.ViewModels;

public class CompeteInitiateVM
{
    public List<UserAchievement> UsersAchievements { get; set; }
    public List<UserAchievement> FriendsAchievements { get; set; }

    public Competition CurrentCompetition { get; set; }

    public CompeteInitiateVM( List<UserAchievement> users, List<UserAchievement> friends ) 
    {
        UsersAchievements = users;
        FriendsAchievements = friends;
    }
}

public class CompeteIndexVM
{
    public List<Competition> Competitions { get; set; }

    public CompeteIndexVM() 
    {
    }
}