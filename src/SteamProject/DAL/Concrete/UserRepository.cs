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
        var user = _ctx.Users
            .Include(u => u.UserBadges)
            .ThenInclude(ub => ub.Badge)
            .Include(u => u.UserGameInfos)
            .Include(u => u.Friends)
            .Include(u => u.UserAchievements)
            .FirstOrDefault(u => u.AspNetUserId == userId);

        if (user == null)
        {
            throw new System.ArgumentNullException();
        }

        return user;
    }



    public IEnumerable<User> GetAllUsers()
    {
        return GetAll().ToList();
    }


}