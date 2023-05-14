
using SteamProject.Models;
using SteamProject.Models.DTO;

namespace SteamProject.ViewModels;


public class CompeteIndexVM
{
    public List<Competition> Competitions { get; set; }

    public List<Competition> CompsComplete { get; set; }
    
    public List<Competition> CompsRunning { get; set; }

    public CompeteIndexVM(){}

}

public class CompeteDetailsVM
{
    public int SinId { get; set; }
    public Competition CurrentComp { get; set; } = null;
    public Game Game { get; set; } = null;
    public List<CompetitionPlayer> CompPlayers { get; set; } = null;
    public List<User> Players { get; set; } = null;
    public List<CompetitionGameAchievement> CompGameAchList { get; set; } = null;
    public List<GameAchievement> GameAchList { get; set; } = null;
    public List<KeyValuePair<UserAchievement, User>> Tracking { get; set; }
    public List<KeyValuePair<User, CompetitionPlayer>> Scoreboard { get; set; }
}


public class CompeteInitiateVM
{
    public List<UserAchievement> UsersAchievements { get; set; } = new List<UserAchievement>();
    public List<UserAchievement> FriendsAchievements { get; set; } = new List<UserAchievement>();
    public Competition CurrentCompetition { get; set; } = new Competition();
    public Game ChosenGame { get; set; } = new Game();
    public string MySteamId { get; set; }
    public string MyFriendId { get; set; }

    public CompeteInitiateVM(){}

    public CompeteInitiateVM( List<UserAchievement> users, List<UserAchievement> friends ) 
    {
        UsersAchievements = users;
        FriendsAchievements = friends;
    }
}


public class CompeteCreateVM
{
    public string SteamId { get; set; }
    public int SinId { get; set; }

    public string OpponentId { get; set; }
    public List<string> OpponentIds { get; set; }

    public int GameAppId { get; set; }

    public string MinDate { get; set; } = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH:mm");


    public List<string> AchievementDisplayNames { get; set; }
    
    public DateTime CompStartTime { get; set; }
    public DateTime CompEndTime { get; set; }

}