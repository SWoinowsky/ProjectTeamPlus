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

}