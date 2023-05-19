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
        private readonly IGameRepository _gameRepository;

        public VoteController(UserManager<IdentityUser> userManager, ICompetitionVoteRepository competitionVoteRepository, IGameVoteRepository gameVoteRepository, IUserRepository userRepository, ICompetitionRepository competitionRepository, IStatusRepository statusRepository, IGameRepository gameRepository)
        {
            _userManager = userManager;
            _competitionVoteRepository = competitionVoteRepository;
            _gameVoteRepository = gameVoteRepository;
            _userRepository = userRepository;
            _competitionRepository = competitionRepository;
            _statusRepository = statusRepository;
            _gameRepository = gameRepository;
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

        [HttpGet]
        [Route("SharedGames/{competitionId}")]
        public IActionResult GetSharedGames(int competitionId)
        {
            var sharedGames = _competitionRepository.GetSharedGames(competitionId);
            if (sharedGames == null)
            {
                return NotFound();
            }

            var sharedGamesDto = sharedGames.Select(game => new GameDto
            {
                Id = game.Id,
                Name = game.Name,
                IconUrl = game.IconUrl
            });

            return Ok(sharedGamesDto);
        }




        [HttpPut("GameVote")]
        public async Task<ActionResult> AddOrUpdateGameVote([FromBody] GameVotePOCO voteData)
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

            // Fetch the game
            var game = _gameRepository.FindById(voteData.GameId);
            if (game is null)
            {
                return BadRequest("Game not found");
            }

            // Check if a vote by this user for this game already exists
            var existingVote = _gameVoteRepository.GetByUserAndGame(user.Id, voteData.GameId);

            if (existingVote == null)
            {
                // No vote exists, so create a new one
                // No vote exists, so create a new one
                existingVote = new GameVote()
                {
                    GameId = voteData.GameId,
                    UserId = user.Id,
                    Vote = voteData.WantsToPlay // include this line
                };

            }

            _gameVoteRepository.AddOrUpdate(existingVote);

            existingVote.User = null;
            existingVote.Game = null;

            // return the updated vote
            return Ok(existingVote);
        }


    }
}
