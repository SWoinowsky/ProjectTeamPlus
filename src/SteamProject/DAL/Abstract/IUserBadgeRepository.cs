using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface IUserBadgeRepository : IRepository<UserBadge>
{
    Task<bool> UserHasBadgeAsync(int userId, string badgeName);
}