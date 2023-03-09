using SteamProject.Models;
using SteamProject.ViewModels;

namespace SteamProject.Services;

public interface ISteamService
{
    User GetSteamUser(string steamid);
    int GetUserLevel(string steamid);
    List<Friend> GetFriendsList(string steamid, int userId);
    IEnumerable<Game> GetGames(string userSteamId, int userId);
    GameVM GetGameInfo(Game game);
    AchievementRoot GetAchievements(string userSteamId, int appId);
    SchemaRoot GetSchema(int appId);
}