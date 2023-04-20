using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface IBadgeRepository : IRepository<Badge>
{
    Task AwardBadgeAsync(User user, int badgeId);

}