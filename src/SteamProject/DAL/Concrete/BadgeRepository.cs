using SteamProject.DAL.Abstract;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;


public class BadgeRepository : Repository<Badge>,  IBadgeRepository
{
    private readonly SteamInfoDbContext _context;
    private readonly IUserBadgeRepository _userBadgeRepository;

    public BadgeRepository(SteamInfoDbContext ctx, IUserBadgeRepository userBadgeRepository) : base(ctx)
    {
        _context = ctx;
        _userBadgeRepository = userBadgeRepository;
    }


    public async Task AwardBadgeAsync(User user, int badgeId)
    {
        var userBadge = new UserBadge { UserId = user.Id, BadgeId = badgeId };
        

        await _context.SaveChangesAsync();
    }
}
