
using SteamProject.Models;

namespace SteamProject.ViewModels;

public class CompeteVM
{
    public List<UserAchievement> UsersAchievements { get; set; }
    public List<UserAchievement> FriendsAchievements { get; set; }

    public CompeteVM( List<UserAchievement> users, List<UserAchievement> friends ) 
    {
        UsersAchievements = users;
        FriendsAchievements = friends;
    }
}