using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface ISpeedRunRepository : IRepository<SpeedRun>
{
    List<SpeedRun> GetAllSpeedRunsForComp(int compId);
    IEnumerable<string> GetSpeedRunsForValidation();
}