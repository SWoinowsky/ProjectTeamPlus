using SteamProject.DAL.Abstract;
using SteamProject.Models.Awards.Abstract;
using SteamProject.Models.Awards.Concrete;

namespace SteamProject.Models.Awards.Concrete;


using System.Threading.Tasks;
public class SteamAccountLinkedCondition : IAwardCondition
{
    public string BadgeName => "Nexus Newcomer";

    public async Task<bool> IsFulfilledAsync(User user, IUserBadgeRepository userBadgeRepository)
    {
        // Check if the user has already been awarded the badge
        if (await userBadgeRepository.UserHasBadgeAsync(user.Id, BadgeName))
        {
            return false;
        }

        // Check if user has steamId registered
        return await Task.FromResult(!string.IsNullOrEmpty(user.SteamId));
    }
}