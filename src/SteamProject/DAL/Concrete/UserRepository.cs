using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
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

                    //These are meant to make creating new badges easier by eager loading these tables for the user
                    // Explicitly load UserBadges collection
                    _ctx.Entry(currentUser).Collection(u => u.UserBadges).Query().Include(ub => ub.Badge).Load();
                    // Explicitly load UserGameInfos 
                    _ctx.Entry(currentUser).Collection(u => u.UserGameInfos).Query().Load();
                    // Explicitly load Friends 
                    _ctx.Entry(currentUser).Collection(u => u.Friends).Query().Load();
                    // Explicitly load UserAchievements
                    _ctx.Entry(currentUser).Collection(u => u.UserAchievements).Query().Load();
                    
                    break;
                }
            }
        }
        else
        {
            throw new System.ArgumentNullException();
        }

        return currentUser;
    }
    public void UpdateUserTheme(int userId, string theme)
    {
        var user = _ctx.Users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            user.Theme = theme;
            _ctx.SaveChanges();
        }
    }




    public IEnumerable<User> GetAllUsers()
    {
        return GetAll().ToList();
    }


}