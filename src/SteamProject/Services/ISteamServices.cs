using SteamProject.Models;

namespace SteamProject.Services
{
    public interface ISteamServices
    {
        /// <summary>
        /// Sets the token which is the API access token so we can hit Steam's API.
        /// </summary>
        /// <param name="token">The token to access the API</param>
        /// <returns>Nothing, it's just setting a key</returns>
        void SetCredentials(string token);

        /// <summary>
        /// Gets the list of games, and assigns them the currently logged in user.
        /// </summary>
        /// <param name="userSteamId">The Steam related Id for the user</param>
        /// <param name="userId">The DB related Id for the user</param>
        /// <param name="user">The User stored in the DB to assign a game to them</param>
        /// <returns>Nothing, it's just setting a key</returns>
        IEnumerable<Game> GetGames(string userSteamId, int userId, User user);

        Game GetGameDescription(Game game);

        AchievementRoot GetAchievements(string userSteamId, int appId);
        SchemaRoot GetSchema(int appId);
    }
}