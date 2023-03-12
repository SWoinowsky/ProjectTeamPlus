using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class UserAchievementRepository : Repository<UserAchievement>,  IUserAchievementRepository
{
    public UserAchievementRepository(SteamInfoDbContext ctx) : base(ctx)
    {

    }

    public List<UserAchievement> GetAchievementsByGameAndUserId( int gameId, int userId )
    {
        return GetAll().Where( ua => ua.Achievement.GameId == gameId && ua.OwnerId == userId ).ToList();
    }
}