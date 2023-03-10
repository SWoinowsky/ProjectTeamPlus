using SteamProject.Models;
using SteamProject.ViewModels;

namespace SteamProject.Services;

public interface ISteamService
{
    User GetSteamUser(string steamid);
    int GetUserLevel(string steamid);
    List<Friend> GetFriendsList(string steamid, int userId);
    public Friend GetFriendSpecific( string userSteamId, int userId, string friendSteamId );
    IEnumerable<Game> GetGames(string userSteamId, int userId);
    public GameNewsVM GetGameNews(Game game);
    GameVM GetGameInfo(Game game);
    AchievementRoot GetAchievements(string userSteamId, int appId);
    SchemaRoot GetSchema(int appId);
}