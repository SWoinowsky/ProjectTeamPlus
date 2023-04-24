using SteamProject.DAL.Abstract;

namespace SteamProject.Models.Awards.Concrete;

public class FollowedThreeGamesCondition : IAwardCondition
{
    public string BadgeName => "Dedicated Follower";

    public async Task<bool> IsFulfilledAsync(User user, IUserBadgeRepository userBadgeRepository)
    {
        // Check if the user has already been awarded the badge
        if (await userBadgeRepository.UserHasBadgeAsync(user.Id, BadgeName))
        {
            return false;
        }

        // Check if user has followed at least 3 games
        return await Task.FromResult(user.UserGameInfos.Count(info => info.Followed) >= 3);
    }
}