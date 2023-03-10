using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class GameAchievementRepository : Repository<GameAchievement>,  IGameAchievementRepository
{
    public GameAchievementRepository(SteamInfoDbContext ctx) : base(ctx)
    {

    }

    public List<GameAchievement> GetAchievementsFromGameId( int gameId )
    {
        return GetAll().Where( gA => gA.GameId == gameId ).ToList();
    }
}