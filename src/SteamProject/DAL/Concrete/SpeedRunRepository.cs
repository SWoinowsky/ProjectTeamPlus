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

    public TimeSpan ConvertRunToTimeSpan(string runTime)
    {
        string[] components = runTime.Split(':');
        
        int hours = int.Parse(components[0]);
        int minutes = int.Parse(components[1]);
        int seconds = int.Parse(components[2]);
        int milliseconds = int.Parse(components[3]);
        
        return new TimeSpan(0, hours, minutes, seconds, milliseconds);
    }

    public IEnumerable<string> GetSpeedRunsForValidation()
    {
        return null;
    }
}