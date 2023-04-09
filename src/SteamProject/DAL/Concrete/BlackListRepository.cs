using SteamProject.DAL.Abstract;
using SteamProject.Data;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class BlackListRepository : Repository<BlackList>, IBlackListRepository
{
    public BlackListRepository(SteamInfoDbContext ctx) : base(ctx)
    {
    }
    public HashSet<string> GetBlackList()
    {
        HashSet<string> blackList = new HashSet<string>();

        return blackList;
    }
}