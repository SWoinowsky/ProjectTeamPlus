using SteamProject.DAL.Abstract;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class FriendRepository : Repository<Friend>, IFriendRepository
{
    public FriendRepository(SteamInfoDbContext ctx) : base(ctx)
    {

    }

    public List<Friend> GetFriends(int userId)
    {
        return GetAll().Where(x => x.RootId == userId ).ToList<Friend>();
    }
}