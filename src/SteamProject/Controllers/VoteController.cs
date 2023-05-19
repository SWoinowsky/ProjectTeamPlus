using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;
using SteamProject.Models.DTO;

namespace SteamProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICompetitionVoteRepository _competitionVoteRepository;
        private readonly IGameVoteRepository _gameVoteRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IStatusRepository _statusRepository;

        public VoteController(UserManager<IdentityUser> userManager, ICompetitionVoteRepository competitionVoteRepository, IGameVoteRepository gameVoteRepository, IUserRepository userRepository, ICompetitionRepository competitionRepository, IStatusRepository statusRepository)
        {
            _userManager = userManager;
            _competitionVoteRepository = competitionVoteRepository;
            _gameVoteRepository = gameVoteRepository;
            _userRepository = userRepository;
            _competitionRepository = competitionRepository;
            _statusRepository = statusRepository;
        }

        [HttpPut("CompetitionVote")]
        public async Task<ActionResult> AddOrUpdateCompetitionVote([FromBody] CompetitionVotePOCO voteData)
        {
            string? userId = _userManager.GetUserId(User); // Get current user's Id

            if (userId is null)
            {
                return BadRequest();
            }

            User user = _userRepository.GetUser(userId);

            if (user is null)
            {
                return BadRequest();
            }

            // Fetch the competition
            var competition = _competitionRepository.FindById(voteData.CompetitionId);
            if (competition is null)
            {
                return BadRequest("Competition not found");
            }

            // Check if a vote by this user for this competition already exists
            var existingVote = _competitionVoteRepository.GetByUserAndCompetition(user.Id, voteData.CompetitionId);

            if (existingVote == null)
            {
                // No vote exists, so create a new one
                existingVote = new CompetitionVote()
                {
                    CompetitionId = voteData.CompetitionId,
                    UserId = user.Id,
                    WantsToPlayAgain = voteData.WantsToPlayAgain,
                };

                // If this is the first vote for this competition, update the competition status to Voting
                if (competition.Status == _statusRepository.GetStatusByName("Ended"))
                {
                    competition.Status = _statusRepository.GetStatusByName("Voting");
                    _competitionRepository.AddOrUpdate(competition);
                }
            }
            else
            {
                // A vote exists, so update it
                existingVote.WantsToPlayAgain = voteData.WantsToPlayAgain;
            }

            _competitionVoteRepository.AddOrUpdate(existingVote);

            existingVote.User = null;
            existingVote.Competition = null;

            // return the updated vote
            return Ok(existingVote);
        }

        [HttpGet("CompetitionVoteCount/{competitionId}")]
        public async Task<ActionResult> GetCompetitionVoteCount(int competitionId)
        {
            var voteCount = _competitionVoteRepository.GetVoteCountForCompetition(competitionId);
            return Ok(voteCount);
        }

        [HttpGet("TotalCompetitionUsers/{competitionId}")]
        public async Task<ActionResult> GetTotalCompetitionUsers(int competitionId)
        {

            var totalUsers = _competitionRepository.GetTotalUsers(competitionId);

            if (totalUsers == null)
            {
                return BadRequest();
            }

            return Ok(totalUsers);
        }




        [HttpPost("GameVote")]
        public async Task<ActionResult> CreateGameVote([FromBody] GameVotePOCO newVote)
        {
            string? userId = _userManager.GetUserId(User); // Get current user's Id

            if (userId is null)
            {
                return BadRequest();
            }

            User user = _userRepository.GetUser(userId);

            if (user is null)
            {
                return BadRequest();
            }

            // Check if a vote by this user for this game already exists
            var existingVote = _gameVoteRepository.GetByUserAndGame(user.Id, newVote.GameId);

            if (existingVote != null)
            {
                return BadRequest("You've already voted for this game.");
            }

            var vote = new GameVote()
            {
                GameId = newVote.GameId,
                UserId = user.Id,
            };

            _gameVoteRepository.AddOrUpdate(vote);

            return Ok();
        }



        [HttpPut("GameVote/{id}")]
        public async Task<ActionResult> UpdateGameVote(int id, [FromBody] GameVotePOCO updatedVote)
        {
            string? userId = _userManager.GetUserId(User); // Get current user's Id

            if (userId is null)
            {
                return BadRequest();
            }

            User user = _userRepository.GetUser(userId);

            if (user is null)
            {
                return BadRequest();
            }

            // Check if the vote exists and the id matches
            var existingVote = _gameVoteRepository.FindById(id);
            if (existingVote == null)
            {
                return BadRequest();
            }


            _gameVoteRepository.AddOrUpdate(existingVote);

            return Ok();
        }

    }
}
