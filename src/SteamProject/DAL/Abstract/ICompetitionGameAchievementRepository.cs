using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface ICompetitionGameAchievementRepository : IRepository<CompetitionGameAchievement>
{
    public List<CompetitionGameAchievement> GetByCompetitionId( int id );

    public List<CompetitionGameAchievement> GetByCompetitionIdAndGameId(int competitionId, int gameId);

    public void EnsureCompetitionGameAchievements(int competitionId, int gameId);

    public void ClearCompetitionGameAchievements(int competitionId);
}