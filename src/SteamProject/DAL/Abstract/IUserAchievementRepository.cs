using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface IUserAchievementRepository : IRepository<UserAchievement>
{
    public List<UserAchievement> GetAchievementsByGameAndUserId( int gameId, int userId );
}