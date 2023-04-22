using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class UserBadgeRepository : Repository<UserBadge>, IUserBadgeRepository
{
    private readonly SteamInfoDbContext _context;

    public UserBadgeRepository(SteamInfoDbContext ctx) : base(ctx)
    {
        _context = ctx;
    }
    public async Task<bool> UserHasBadge(int userId, int badgeId)
    {
        return await _context.UserBadges.AnyAsync(ub => ub.UserId == userId && ub.BadgeId == badgeId);
    }

}