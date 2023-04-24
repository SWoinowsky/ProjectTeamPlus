using SteamProject.DAL.Abstract;

namespace SteamProject.Models.Awards.Concrete;

public class Level10Condition : IAwardCondition
{
    public string BadgeName => "Rising Star";

    public async Task<bool> IsFulfilledAsync(User user, IUserBadgeRepository userBadgeRepository)
    {
        // Check if the user has already been awarded the badge
        if (await userBadgeRepository.UserHasBadgeAsync(user.Id, BadgeName))
        {
            return false;
        }

        // Check if user has reached at least level 10
        return await Task.FromResult(user.PlayerLevel.HasValue && user.PlayerLevel.Value >= 10);
    }
}