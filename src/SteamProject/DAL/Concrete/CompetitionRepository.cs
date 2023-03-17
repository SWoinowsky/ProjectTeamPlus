using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class CompetitionRepository : Repository<Competition>,  ICompetitionRepository
{
    public CompetitionRepository(SteamInfoDbContext ctx) : base(ctx)
    {

    }

    public Competition GetCompetitionById(int id)
    {
        return GetAll().Where( c => c.Id == id ).FirstOrDefault();
    }
}