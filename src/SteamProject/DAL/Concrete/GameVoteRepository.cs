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

        public GameVote GetByUserAndGame(int userId, int gameId)
        {
            return _ctx.GameVotes
                .FirstOrDefault(gv => gv.UserId == userId && gv.GameId == gameId);
        }

    }
}