using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class CompetitionRepository : Repository<Competition>,  ICompetitionRepository
{
    private readonly ICompetitionPlayerRepository _competitionPlayerRepository;

    public CompetitionRepository(SteamInfoDbContext ctx, ICompetitionPlayerRepository competitionPlayerRepository) : base(ctx)
    {
        _competitionPlayerRepository = competitionPlayerRepository;
    }

    public Competition GetCompetitionById(int id)
    {
        return GetAll().Where( c => c.Id == id ).FirstOrDefault();
    }

    public Competition GetCompetitionByCompPlayerAndGameId( CompetitionPlayer player, int gameId )
    {
        return GetAll().Where( c => c.GameId == gameId && c.CompetitionPlayers.Contains( player ) ).FirstOrDefault();
    }

    public List<Competition> GetAllCompetitionsForUser( List<CompetitionPlayer> entries )
    {
        if ( entries == null || entries.Count() == 0 )
            return null;
        
        var returnMe = new List<Competition>();
        foreach( var competitionPlayer in entries )
        {
            var CompsFound = new List<Competition>();
            CompsFound = GetAll().Where( c => c.CompetitionPlayers.Contains( competitionPlayer ) ).ToList<Competition>();
            foreach( var comp in CompsFound )
                returnMe.Add( comp );
        }

        return returnMe;
    }

    public List<Competition> GetCurrentCompetitionsBySteamId(string steamId)
    {
        // Get all competition players for the user
        var competitionPlayers = _competitionPlayerRepository.GetCompetitionIdsBySteamId(steamId);

        if (competitionPlayers == null)
        {
            return null;
        }

        // Get the current date
        var currentDate = DateTime.Now;

        // Get all competitions for the competition players that are currently ongoing
        return GetAllCompetitionsForUser(competitionPlayers)
            .Where(c => c.StartDate <= currentDate && c.EndDate >= currentDate)
            .ToList();
    }

    public List<Competition> GetPreviousCompetitionsBySteamId(string steamId)
    {
        // Get all competition players for the user
        var competitionPlayers = _competitionPlayerRepository.GetCompetitionIdsBySteamId(steamId);

        if (competitionPlayers == null)
        {
            return null;
        }

        // Get the current date
        var currentDate = DateTime.Now;

        // Get all competitions for the competition players that have ended
        return GetAllCompetitionsForUser(competitionPlayers)
            .Where(c => c.EndDate < currentDate)
            .ToList();
    }

}