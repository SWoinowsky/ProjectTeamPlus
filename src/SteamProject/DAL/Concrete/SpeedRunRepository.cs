using SteamProject.DAL.Abstract;
using SteamProject.Data;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class SpeedRunRepository : Repository<SpeedRun>, ISpeedRunRepository
{
    public SpeedRunRepository(SteamInfoDbContext ctx) : base(ctx)
    {
    }

    public List<SpeedRun> GetAllSpeedRunsForComp(int compId)
    {
        return GetAll().Where(s => s.CompetitionId == compId).ToList();
    }
    
    public IEnumerable<string> GetSpeedRunsForValidation()
    {
        return null;
    }
}