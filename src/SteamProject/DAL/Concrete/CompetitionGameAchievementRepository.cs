using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class CompetitionGameAchievementRepository : Repository<CompetitionGameAchievement>,  ICompetitionGameAchievementRepository
{
    public CompetitionGameAchievementRepository(SteamInfoDbContext ctx) : base(ctx)
    {

    }

    public List<CompetitionGameAchievement> GetByCompetitionId( int id )
    {
        var returnMe = new List<CompetitionGameAchievement>();
        returnMe = GetAll().Where( cga => cga.CompetitionId == id ).ToList<CompetitionGameAchievement>();

        if( returnMe.Count() == 0 )
            return null;
        else
            return returnMe;
    }
}