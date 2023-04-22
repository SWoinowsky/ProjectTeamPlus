using SteamProject.DAL.Abstract;
using SteamProject.Models.Awards.Concrete;

namespace SteamProject.Models.Awards.Concrete;


using System.Threading.Tasks;

public class SteamAccountLinkedCondition : IAwardCondition
{
    public string BadgeName => "Nexus Newcomer";
    public async Task<bool> IsFulfilledAsync(User user)
    {
        // Check if user has steamId registered
        return await Task.FromResult(!string.IsNullOrEmpty(user.SteamId));
    }

    
}
