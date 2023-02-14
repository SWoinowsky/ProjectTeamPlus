using SteamProject.DAL.Abstract;
using SteamProject.Data;
using SteamProject.Models;

// Put this in folder DAL/Concrete
namespace SteamProject.DAL.Concrete;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(SteamInfoDbContext ctx) : base(ctx)
    {

    }



}