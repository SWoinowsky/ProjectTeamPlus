using SteamProject.DAL.Abstract;
using SteamProject.Data;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class GenreRepository : Repository<Genre>, IGenreRepository
{
    public GenreRepository(SteamInfoDbContext ctx) : base(ctx)
    {
    }

    public IEnumerable<string> GetGenreList()
    {
        List<string> genres = new List<string>();
        foreach(var item in GetAll())
        {
            genres.Add(item.Name);
        }
        return genres;
    }
}