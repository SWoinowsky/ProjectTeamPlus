using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface IGameVoteRepository : IRepository<GameVote>
{

    public Task AddVoteAsync(GameVote vote);

    public Task UpdateVoteAsync(GameVote vote);

    public GameVote GetByUserAndGame(int userId, int gameId, int voteDataCompetitionId);
    int GetVoteCountForGame(int gameId, int competitionId);

    bool HasGameVoteSucceeded(int competitionInId);
    int GetGameIdWithMostVotes(int compId);

    public void ClearVotes(int competitionId);
}