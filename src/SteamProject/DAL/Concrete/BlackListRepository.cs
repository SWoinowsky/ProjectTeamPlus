using SteamProject.DAL.Abstract;
using SteamProject.Data;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class BlackListRepository : Repository<BlackList>, IBlackListRepository
{
    public BlackListRepository(SteamInfoDbContext ctx) : base(ctx)
    {
    }
    public IEnumerable<string> GetBlackList()
    {
        List<string> blacklist = new List<string>();
        foreach(var item in GetAll())
        {
            blacklist.Add(item.SteamId);
        }
        return blacklist;
    }

    public bool CheckForBlackList(string id)
    {
        var check = false;
        foreach(var item in GetAll())
        {
            if(item.SteamId == id)
            {
                check = true;
                break;
            }
        }
        return check;
    }
}