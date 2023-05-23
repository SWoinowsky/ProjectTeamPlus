using SteamProject.DAL.Abstract;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete
{
    public class GameVoteRepository : Repository<GameVote>, IGameVoteRepository
    {
        private readonly SteamInfoDbContext _ctx;

        public GameVoteRepository(SteamInfoDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task AddVoteAsync(GameVote vote)
        {
            _ctx.GameVotes.Add(vote);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateVoteAsync(GameVote vote)
        {
            _ctx.GameVotes.Update(vote);
            await _ctx.SaveChangesAsync();
        }

        public GameVote GetByUserAndGame(int userId, int gameId, int competitionId)
        {
            return _ctx.GameVotes
                .FirstOrDefault(gv => gv.UserId == userId && gv.GameId == gameId && gv.CompetitionId == competitionId);
        }
        public int GetVoteCountForGame(int gameId, int competitionId)
        {
            return _ctx.GameVotes.Count(gv => gv.GameId == gameId && gv.CompetitionId == competitionId && gv.Vote == true);
        }

        public bool HasGameVoteSucceeded(int competitionId)
        {
            // Get total number of participants in this competition
            int totalParticipants = _ctx.CompetitionPlayers.Where(cp => cp.CompetitionId == competitionId).Count();

            // Get total number of votes for the winning game
            int gameIdWithMostVotes = _ctx.GameVotes
                .Where(gv => gv.CompetitionId == competitionId && gv.Vote == true)
                .GroupBy(gv => gv.GameId)
                .OrderByDescending(grp => grp.Count())
                .Select(grp => grp.Key)
                .FirstOrDefault();

            int votesForWinningGame = _ctx.GameVotes
                .Count(gv => gv.CompetitionId == competitionId && gv.GameId == gameIdWithMostVotes && gv.Vote == true);

            // A game vote is successful if the winning game has more than 50% of the total participants
            return votesForWinningGame > totalParticipants / 2;
        }

        public int GetGameIdWithMostVotes(int competitionId)
        {
            // Get the game ID with the most votes in the given competition
            int gameIdWithMostVotes = _ctx.GameVotes
                .Where(gv => gv.CompetitionId == competitionId && gv.Vote == true)
                .GroupBy(gv => gv.GameId)
                .OrderByDescending(grp => grp.Count())
                .Select(grp => grp.Key)
                .FirstOrDefault();

            return gameIdWithMostVotes;

        }
    }
}