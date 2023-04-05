using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface ICompetitionGameAchievementRepository : IRepository<CompetitionGameAchievement>
{
    public List<CompetitionGameAchievement> GetByCompetitionId( int id );
}