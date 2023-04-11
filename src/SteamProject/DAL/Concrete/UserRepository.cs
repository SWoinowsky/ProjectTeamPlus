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

    public User GetUser(string userId)
    {
        var currentUser = new User();
        var users = GetAll().ToList();
        if(users.Count() > 0)
        {
            foreach(var user in users)
            {
                if(user.AspNetUserId == userId)
                {
                    return user;
                }
            }
        }
        else
        {
            throw new System.ArgumentNullException();
        }
        return null;
    }

}