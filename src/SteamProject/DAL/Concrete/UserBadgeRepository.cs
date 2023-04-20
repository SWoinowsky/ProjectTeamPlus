using SteamProject.DAL.Abstract;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class UserBadgeRepository : Repository<UserBadge>, IUserBadgeRepository
{
    private readonly SteamInfoDbContext _context;
    private readonly IUserBadgeRepository _userBadgeRepository;

    public UserBadgeRepository(SteamInfoDbContext ctx, IUserBadgeRepository userBadgeRepository) : base(ctx)
    {
        _context = ctx;
        _userBadgeRepository = userBadgeRepository;
    }

}