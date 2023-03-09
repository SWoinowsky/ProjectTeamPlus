using SteamProject.DAL.Abstract;
using SteamProject.Data;
using SteamProject.Models;

// Put this in folder DAL/Concrete
namespace SteamProject.DAL.Concrete;

public class UserGameInfoRepository : Repository<UserGameInfo>, IUserGameInfoRepository
{
    public UserGameInfoRepository(SteamInfoDbContext ctx) : base(ctx)
    {

    }

    public UserGameInfo? GetUserInfoForGame(int gameAppId, int userId)
    {
        return this.GetAll(g => g.Game.AppId == gameAppId).Where(g => g.OwnerId == userId ).SingleOrDefault();
    }

    public List<UserGameInfo> GetAllUserGameInfo(int userId)
    {
        return this.GetAll(g => g.OwnerId == userId).ToList();
    }
}