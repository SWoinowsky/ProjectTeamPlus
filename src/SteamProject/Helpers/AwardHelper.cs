using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.Models.Awards.Abstract;

namespace SteamProject.Helpers;

public class AwardHelper
{
    private readonly IUserBadgeRepository _userBadgeRepository;
    private readonly IBadgeRepository _badgeRepository;

    public AwardHelper(IUserBadgeRepository userBadgeRepository, IBadgeRepository badgeRepository)
    {
        _userBadgeRepository = userBadgeRepository;
        _badgeRepository = badgeRepository;
    }

    public async Task CheckAndAwardAsync(User user, IAwardCondition condition, int badgeId)
    {
        if (await _badgeRepository.BadgeExistsAsync(badgeId) && await condition.IsFulfilledAsync(user))
        {
            await _badgeRepository.AwardBadgeAsync(user, badgeId);
        }
    }

}