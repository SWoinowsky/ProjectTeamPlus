using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class CompetitionGameAchievementRepository : Repository<CompetitionGameAchievement>,  ICompetitionGameAchievementRepository
{
    
    private readonly SteamInfoDbContext _ctx;
    private readonly IGameAchievementRepository _gameAchievementRepository;
    public CompetitionGameAchievementRepository(SteamInfoDbContext ctx, IGameAchievementRepository gameAchievementRepository) : base(ctx)
    {
        _ctx = ctx;
        _gameAchievementRepository = gameAchievementRepository;
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

    public List<CompetitionGameAchievement> GetByCompetitionIdAndGameId(int competitionId, int gameId)
    {
        var returnMe = new List<CompetitionGameAchievement>();

        // We are joining with the GameAchievements to make sure we only fetch the achievements for the correct game
        returnMe = GetAll()
            .Join(_ctx.GameAchievements,
                cga => cga.GameAchievementId,
                ga => ga.Id,
                (cga, ga) => new { CGA = cga, GA = ga })
            .Where(x => x.CGA.CompetitionId == competitionId && x.GA.GameId == gameId)
            .Select(x => x.CGA)
            .ToList();

        if (returnMe.Count() == 0)
            return null;
        else
            return returnMe;
    }

    public void EnsureCompetitionGameAchievements(int competitionId, int gameId)
    {
        // Check if there are already achievements for this game in this competition
        var existingAchievements = _ctx.CompetitionGameAchievements.Where(cga => cga.CompetitionId == competitionId && cga.GameAchievementId == gameId);

        // If there are already achievements, clear out the old ones
        if (existingAchievements.Any())
        {
            foreach (var achievement in existingAchievements)
            {
                _ctx.CompetitionGameAchievements.Remove(achievement);
            }
            _ctx.SaveChanges();
        }


        // Fetch the game's achievements
        var gameAchievements = _gameAchievementRepository.GetAchievementsFromGameId(gameId);

        // If there are no achievements for this game, we can't add any to the competition
        if (gameAchievements == null || !gameAchievements.Any())
        {
            return;
        }

        // Add each game achievement to the competition
        foreach (var gameAchievement in gameAchievements)
        {
            var competitionGameAchievement = new CompetitionGameAchievement
            {
                CompetitionId = competitionId,
                GameAchievementId = gameAchievement.Id
            };

            _ctx.CompetitionGameAchievements.Add(competitionGameAchievement);
        }

        _ctx.SaveChanges();
    }


}