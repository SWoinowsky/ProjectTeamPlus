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




}