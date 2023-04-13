using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface IBlackListRepository : IRepository<BlackList>
{
    IEnumerable<string> GetBlackList();
    bool CheckForBlackList(string id);
}