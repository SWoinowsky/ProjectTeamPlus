using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface IGameAchievementRepository : IRepository<GameAchievement>
{
    public List<GameAchievement> GetAchievementsFromGameId( int gameId );
}