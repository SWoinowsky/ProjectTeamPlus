using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface ICompetitionPlayerRepository : IRepository<CompetitionPlayer>
{
    public List<int> GetCompetitionIdsBySteamId( int id );
}