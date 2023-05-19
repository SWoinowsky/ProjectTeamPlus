using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;

namespace SteamProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly ICompetitionVoteRepository _competitionVoteRepository;
        private readonly IGameVoteRepository _gameVoteRepository;

        public VoteController(CompetitionVoteRepository competitionVoteRepository, GameVoteRepository gameVoteRepository  )
        {
            _competitionVoteRepository = competitionVoteRepository;
            _gameVoteRepository = gameVoteRepository;
        }

        [HttpPost("competitionvote")]
        public async Task<ActionResult> CreateCompetitionVote(CompetitionVote vote)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current user's Id
            if (vote.UserId != int.Parse(userId))
            {
                return Unauthorized();
            }

            await _competitionVoteRepository.AddVoteAsync(vote);
            return Ok();
        }

        [HttpPut("competitionvote/{id}")]
        public async Task<ActionResult> UpdateCompetitionVote(int id, CompetitionVote vote)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current user's Id
            if (vote.UserId != int.Parse(userId) || id != vote.Id)
            {
                return Unauthorized();
            }

            await _competitionVoteRepository.UpdateVoteAsync(vote);
            return Ok();
        }

        [HttpPost("gamevote")]
        public async Task<ActionResult> CreateGameVote(GameVote vote)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current user's Id
            if (vote.UserId != int.Parse(userId))
            {
                return Unauthorized();
            }

            await _gameVoteRepository.AddVoteAsync(vote);
            return Ok();
        }

        [HttpPut("gamevote/{id}")]
        public async Task<ActionResult> UpdateGameVote(int id, GameVote vote)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current user's Id
            if (vote.UserId != int.Parse(userId) || id != vote.Id)
            {
                return Unauthorized();
            }

            await _gameVoteRepository.UpdateVoteAsync(vote);
            return Ok();
        }
    }
}
