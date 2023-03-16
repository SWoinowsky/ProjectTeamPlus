using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface ICompetitionRepository : IRepository<Competition>
{
    public Competition GetCompetitionById( int id );
}