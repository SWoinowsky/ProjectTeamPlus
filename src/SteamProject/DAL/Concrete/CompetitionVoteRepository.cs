using SteamProject.DAL.Abstract;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class CompetitionVoteRepository : Repository<CompetitionVote>, ICompetitionVoteRepository
{
    private readonly SteamInfoDbContext _ctx;

    public CompetitionVoteRepository(SteamInfoDbContext ctx) : base(ctx)
    {
        _ctx = ctx;
    }

    public async Task AddVoteAsync(CompetitionVote vote)
    {
        _ctx.CompetitionVotes.Add(vote);
        await _ctx.SaveChangesAsync();
    }

    public async Task UpdateVoteAsync(CompetitionVote vote)
    {
        _ctx.CompetitionVotes.Update(vote);
        await _ctx.SaveChangesAsync();
    }
}