using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface ICompetitionPlayerRepository : IRepository<CompetitionPlayer>
{
    public List<CompetitionPlayer> GetCompetitionIdsBySteamId( string id );
}