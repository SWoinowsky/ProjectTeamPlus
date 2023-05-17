using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface IGameVoteRepository : IRepository<GameVote>
{

    public Task AddVoteAsync(GameVote vote);

    public Task UpdateVoteAsync(GameVote vote);

}