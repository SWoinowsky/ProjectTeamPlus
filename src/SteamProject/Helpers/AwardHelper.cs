using SteamProject.Models;

public class AwardHelper
{
    private readonly ApplicationDbContext _context;

    public AwardHelper(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CheckAndAwardAsync(User user, IAwardCondition condition, int badgeId)
    {
        if (condition.IsConditionMet(user))
        {
            await AwardBadgeAsync(user, badgeId);
        }
    }

    private async Task AwardBadgeAsync(User user, int badgeId)
    {
        // Check if the user already has the badge
        var existingBadge = _context.UserBadges.FirstOrDefault(ub => ub.UserId == user.Id && ub.BadgeId == badgeId);

        if (existingBadge == null)
        {
            // Award the badge and save it in the database
            var userBadge = new UserBadge { UserId = user.Id, BadgeId = badgeId };
            _context.UserBadges.Add(userBadge);
            await _context.SaveChangesAsync();
        }
    }
}