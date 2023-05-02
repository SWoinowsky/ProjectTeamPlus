using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface IIGDBGenresRepository : IRepository<Igdbgenre>
{
    IEnumerable<string> GetGenreList();
}
