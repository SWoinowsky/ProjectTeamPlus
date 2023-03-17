using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class CompetitionPlayerRepository : Repository<CompetitionPlayer>,  ICompetitionPlayerRepository
{
    public CompetitionPlayerRepository(SteamInfoDbContext ctx) : base(ctx)
    {
    }

    public List<int> GetCompetitionIdsBySteamId( int id )
    {
        var returnMe = GetAll().Where( c => c.SteamId == id ).Select(c => c.CompetitionId).ToList<int>();
        if( returnMe.Count() == 0 )
            return null;
        else
            return returnMe;
    }

}