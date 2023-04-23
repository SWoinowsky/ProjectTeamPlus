using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.Data;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly SteamInfoDbContext _ctx;

    public UserRepository(SteamInfoDbContext ctx) : base(ctx)
    {
        _ctx = ctx;
    }

    public User GetUser(string userId)
    {
        var currentUser = new User();
        var users = GetAll().ToList();
        if (users.Count() > 0)
        {
            foreach (var user in users)
            {
                if (user.AspNetUserId == userId)
                {
                    currentUser = user;
                    break;
                }
            }
        }
        else
        {
            throw new System.ArgumentNullException();
        }

        // Explicitly load UserBadges collection and include Badge
        _ctx.Entry(currentUser).Collection(u => u.UserBadges).Query().Include(ub => ub.Badge).Load();

        return currentUser;
    }


    public IEnumerable<User> GetAllUsers()
    {
        return GetAll().ToList();
    }


}