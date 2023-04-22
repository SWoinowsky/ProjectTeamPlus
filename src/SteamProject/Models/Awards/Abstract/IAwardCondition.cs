namespace SteamProject.Models.Awards.Abstract;

public interface IAwardCondition
{
    Task<bool> IsFulfilledAsync(User user);
}