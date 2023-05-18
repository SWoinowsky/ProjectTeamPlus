using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;
using SteamProject.ViewModels;

namespace SteamProject.DAL.Abstract
{
    public interface ICompetitionVoteRepository : IRepository<CompetitionVote>
    {
        public Task AddVoteAsync(CompetitionVote vote);


        public Task UpdateVoteAsync(CompetitionVote vote);

        CompetitionVote GetByUserAndCompetition(int userId, int competitionId);


    }
}