using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.Models.DTO;

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
        _userBadgeRepository.AddOrUpdate(userBadge);

    }
    public async Task<bool> BadgeExistsAsync(int badgeId)
    {
        return await _context.Badges.AnyAsync(b => b.Id == badgeId);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}
