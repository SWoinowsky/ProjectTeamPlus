using Microsoft.EntityFrameworkCore;
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

    public CompetitionVote GetByUserAndCompetition(int userId, int competitionId)
    {
        return _ctx.CompetitionVotes
            .FirstOrDefault(cv => cv.UserId == userId && cv.CompetitionId == competitionId);
    }

    public int GetVoteCountForCompetition(int competitionId)
    {
        return _ctx.CompetitionVotes
            .Where(v => v.CompetitionId == competitionId && v.WantsToPlayAgain)
            .Count();
    }

    public int GetPositiveVotesCount(int competitionId)
    {
        return GetAll().Where(vote => vote.CompetitionId == competitionId && vote.WantsToPlayAgain).Count();
    }

    public void ClearVotes(int competitionId)
    {
        var votes = _ctx.CompetitionVotes.Where(cv => cv.CompetitionId == competitionId);
        _ctx.CompetitionVotes.RemoveRange(votes);
        _ctx.SaveChanges();
    }


}