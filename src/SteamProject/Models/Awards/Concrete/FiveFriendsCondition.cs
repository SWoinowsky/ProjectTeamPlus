using SteamProject.DAL.Abstract;
using SteamProject.Models.Awards.Abstract;

namespace SteamProject.Models.Awards.Concrete;

public class FiveFriendsCondition : IAwardCondition
{
    public string BadgeName => "Friendly Face";

    public async Task<bool> IsFulfilledAsync(User user, IUserBadgeRepository userBadgeRepository)
    {
        // Check if the user has already been awarded the badge
        if (await userBadgeRepository.UserHasBadgeAsync(user.Id, BadgeName))
        {
            return false;
        }

        // Check if user has at least 5 friends
        return await Task.FromResult(user.Friends.Count >= 5);
    }
}