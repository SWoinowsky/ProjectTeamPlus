using SteamProject.DAL.Abstract;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class StatusRepository : Repository<Status>, IStatusRepository
{
    private readonly SteamInfoDbContext _context;

    public StatusRepository(SteamInfoDbContext ctx) : base(ctx)
    {
        _context = ctx;
    }
    public Status? GetStatusByName(string name)
    {
        return _context.Statuses.FirstOrDefault(s => s.Name == name);
    }
}