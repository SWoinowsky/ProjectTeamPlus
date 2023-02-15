using SteamProject.Models;

namespace SteamProject.Services
{
    public interface ISteamServices
    {
        IEnumerable<Game> GetGames();
    }
}