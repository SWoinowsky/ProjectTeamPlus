using SteamProject.DAL.Abstract;

namespace SteamProject.Models.Awards.Abstract;

public interface IAwardCondition
{
    string BadgeName { get; }
    Task<bool> IsFulfilledAsync(User user, IUserBadgeRepository userBadgeRepository);
}