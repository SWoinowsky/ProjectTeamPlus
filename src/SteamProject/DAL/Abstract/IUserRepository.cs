using SteamProject.DAL.Abstract;
using SteamProject.Models;

namespace SteamProject.DAL.Abstract
{

    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Get information about the current user.
        /// </summary>
        /// <param name="userId">The identity ID of the current user.</param>
        /// <returns>A new User with all of the info in the User table about them</returns>
        User GetUser(string userId);
    }
}