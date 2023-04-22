using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface IUserBadgeRepository : IRepository<UserBadge>
{
    Task<bool> UserHasBadge(int userId, int badgeId);
}