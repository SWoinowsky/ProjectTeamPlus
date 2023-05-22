using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface ISpeedRunRepository : IRepository<SpeedRun>
{
    List<SpeedRun> GetAllSpeedRunsForComp(int compId);
    TimeSpan ConvertRunToTimeSpan(string runTime);
    IEnumerable<string> GetSpeedRunsForValidation();
}