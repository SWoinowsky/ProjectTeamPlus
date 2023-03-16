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

}