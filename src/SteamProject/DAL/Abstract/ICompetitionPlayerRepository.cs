using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface ICompetitionPlayerRepository : IRepository<CompetitionPlayer>
{
    public List<CompetitionPlayer> GetCompetitionIdsBySteamId( string id );

    public List<CompetitionPlayer> GetAllForCompetition( int id );

}