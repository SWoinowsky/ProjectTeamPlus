using SteamProject.Models;

namespace SteamProject.Services;

public interface ISteamService
{
    User GetSteamUser(string steamid);
    int GetUserLevel(string steamid);
}