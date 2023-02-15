using SteamProject.Models;

namespace SteamProject.Services;

public interface ISteamService
{
    User SteamUser(string steamid);
}