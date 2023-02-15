using SteamProject.DAL.Abstract;
using SteamProject.Models;

namespace SteamProject.DAL.Abstract
{

    public interface IGameRepository : IRepository<Game>
    {
        /// <summary>
        /// Return all the games that a user owns sorted by the appId number
        /// </summary>
        /// <param name="userId">The id of the user that we are fetching games for</param>
        /// <returns> List of games </returns>
        ICollection<Game> GetGames(string userId);
    }
}