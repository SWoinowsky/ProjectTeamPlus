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

    public async Task SeedBadgesAsync()
    {
        if (!await _context.Badges.AnyAsync())
        {
            var badgeDataJson = File.ReadAllText("Data/badges.json");
            var badgeData = JsonConvert.DeserializeObject<List<BadgeData>>(badgeDataJson);

            foreach (var data in badgeData)
            {
                var badge = new Badge
                {
                    Id = data.Id,
                    Name = data.Name,
                    Description = data.Description
                };

                await _context.Badges.AddAsync(badge);
            }

            await _context.SaveChangesAsync();
        }
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}
