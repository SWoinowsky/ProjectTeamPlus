using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface IBlackListRepository : IRepository<BlackList>
{
    HashSet<string> GetBlackList();
}