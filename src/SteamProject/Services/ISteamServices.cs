using SteamProject.Models;

namespace SteamProject.Services
{
    public interface ISteamServices
    {
        void SetCredentials(string token);
        IEnumerable<Game> GetGames(string userId);
    }
}