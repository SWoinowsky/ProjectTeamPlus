using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface ISpeedRunRepository : IRepository<SpeedRun>
{
    IEnumerable<string> GetSpeedRunsForValidation();
}