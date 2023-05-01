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

        /// <summary>
        /// Get a list of all users on the site -- used solely for admin access right now.
        /// <summary>
        /// <returns>A list of all the users on the site.
        IEnumerable<User> GetAllUsers();

        void UpdateUserTheme(int userId, string theme);
    }
}