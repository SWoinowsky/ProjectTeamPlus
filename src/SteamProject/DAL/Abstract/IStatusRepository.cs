using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface IStatusRepository : IRepository<Status>
{
    Status? GetStatusByName(string name);

}