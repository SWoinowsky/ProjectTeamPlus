using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class CompetitionPlayerRepository : Repository<CompetitionPlayer>,  ICompetitionPlayerRepository
{
 
    private readonly ICompetitionVoteRepository _competitionVoteRepository;
    private readonly SteamInfoDbContext _ctx;

    public CompetitionPlayerRepository(SteamInfoDbContext ctx, ICompetitionVoteRepository competitionVoteRepository) : base(ctx)
    {
        _ctx = ctx;
        _competitionVoteRepository = competitionVoteRepository;
    }

    public List<CompetitionPlayer> GetAllForCompetition(int id)
    {
        var returnMe = new List<CompetitionPlayer>();
        returnMe = GetAll().Where( p => p.CompetitionId == id ).ToList<CompetitionPlayer>();

        if( returnMe.Count() == 0 )
            return null;
        else
            return returnMe;
    }

    public List<CompetitionPlayer> GetCompetitionIdsBySteamId( string id )
    {
        var returnMe = GetAll().Where( c => c.SteamId == id ).ToList<CompetitionPlayer>();
        if( returnMe.Count() == 0 )
            return null;
        else
            return returnMe;
    }

    public List<Game> GetGamesForPlayer(CompetitionPlayer player)
    {
        // Fetch the games for the player from UserGameInfo joined with Game on GameId
        return _ctx.UserGameInfos
            .Where(ugi => ugi.OwnerId == player.Id)
            .Include(ugi => ugi.Game)
            .Select(ugi => ugi.Game)
            .ToList();
    }
}