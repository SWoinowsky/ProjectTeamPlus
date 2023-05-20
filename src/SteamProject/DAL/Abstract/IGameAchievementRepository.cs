using SteamProject.DAL.Concrete;
using SteamProject.Models;
using SteamProject.Models.DTO;

namespace SteamProject.DAL.Abstract;

public interface IGameAchievementRepository : IRepository<GameAchievement>
{
    public List<GameAchievement> GetAchievementsFromGameId( int gameId );

    public void EnsureGameAchievements(int appId, string steamId, int userId);


}