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
        [Authorize]
        [HttpGet]
        [Route("SharedGames/{competitionId}")]
        public IActionResult GetSharedGames(int competitionId)
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

            var sharedGames = _competitionRepository.GetSharedGames(competitionId);
            if (sharedGames == null)
            {
                return NotFound();
            }

            var sharedGamesDto = sharedGames.Select(game => new GameVoteDto
            {
                Id = game.Id,
                Name = game.Name,
                AppId = game.AppId,
                CurrentUserVote = _gameVoteRepository.GetByUserAndGame(user.Id, game.Id, competitionId)?.Vote,
                VoteCount = _gameVoteRepository.GetVoteCountForGame(game.Id, competitionId),

            }).ToList();

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
            var existingVote = _gameVoteRepository.GetByUserAndGame(user.Id, voteData.GameId, voteData.CompetitionId);

            if (existingVote == null)
            {
                // No vote exists, so create a new one
                existingVote = new GameVote()
                {
                    GameId = voteData.GameId,
                    UserId = user.Id,
                    Vote = voteData.WantsToPlay, 
                    CompetitionId = voteData.CompetitionId
                };
            }
            else
            {
                // A vote exists, so update it
                existingVote.Vote = voteData.WantsToPlay;
            }

            _gameVoteRepository.AddOrUpdate(existingVote);




            // Get the competition Id associated with this game.
            // You will need to implement this logic based on your data model.
            var competition = _competitionRepository.GetCompetitionById(existingVote.CompetitionId);

            if (competition == null)
            {
                return BadRequest("Competition not found for this game");
            }

            // Get the vote count for the game
            var voteCount = _gameVoteRepository.GetVoteCountForGame(game.Id, existingVote.CompetitionId);

            // Get the total users in the competition
            var totalUsers = _competitionRepository.GetTotalUsers(competition.Id);

            // If the vote count reaches a majority, update the competition with the new game
            if (voteCount > totalUsers / 2)
            {
                var updatedCompetition = _competitionRepository.UpdateGameForCompetition(competition.Id, game.Id);

                if (updatedCompetition == null)
                {
                    return BadRequest("Error updating competition");
                }


            }

            existingVote.User = null;
            existingVote.Game = null;
            existingVote.Competition = null;

            // return the updated vote
            return Ok(existingVote);
        }


        [HttpGet("GameVotes/{gameId}/{competitionId}")]
        public IActionResult GameVotes(int gameId, int competitionId)
        {
            string? userId = _userManager.GetUserId(User); // Get current user's Id

            if (userId is null)
            {
                return BadRequest();
            }

            // Fetch the specific game based on gameId
            var game = _gameRepository.GetGameById(gameId);

            if (game == null)
            {
                return NotFound();
            }

            User user = _userRepository.GetUser(userId);

            // If the game exists, map it to GameVoteDto
            var gameVoteDto = new GameVoteDto
            {
                Id = game.Id,
                Name = game.Name,
                AppId = game.AppId,
                VoteCount = _gameVoteRepository.GetVoteCountForGame(game.Id, competitionId),
                CurrentUserVote = _gameVoteRepository.GetByUserAndGame(user.Id, game.Id, competitionId)?.Vote ?? false
            };

            return Ok(gameVoteDto);
        }


    }
}
