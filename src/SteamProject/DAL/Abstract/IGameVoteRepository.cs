using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;

public interface IGameVoteRepository : IRepository<GameVote>
{

    public Task AddVoteAsync(GameVote vote);

    public Task UpdateVoteAsync(GameVote vote);

}