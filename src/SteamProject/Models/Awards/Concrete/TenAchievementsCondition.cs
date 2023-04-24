using SteamProject.DAL.Abstract;
using SteamProject.Models.Awards.Abstract;

namespace SteamProject.Models.Awards.Concrete;

public class TenAchievementsCondition : IAwardCondition
{
    public string BadgeName => "Achievement Hunter";

    public async Task<bool> IsFulfilledAsync(User user, IUserBadgeRepository userBadgeRepository)
    {
        // Check if the user has already been awarded the badge
        if (await userBadgeRepository.UserHasBadgeAsync(user.Id, BadgeName))
        {
            return false;
        }

        // Check if user has at least 10 achievements
        return await Task.FromResult(user.UserAchievements.Count >= 10);
    }
}