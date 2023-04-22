using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.Helpers;
using SteamProject.Models.Awards.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace SteamProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BadgeController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IBadgeRepository _badgeRepository;
        private readonly IUserBadgeRepository _userBadgeRepository;
        private readonly IEnumerable<IAwardCondition> _awardConditions;

        public BadgeController(IUserRepository userRepository, IBadgeRepository badgeRepository, IUserBadgeRepository userBadgeRepository, IEnumerable<IAwardCondition> awardConditions)
        {
            _userRepository = userRepository;
            _badgeRepository = badgeRepository;
            _userBadgeRepository = userBadgeRepository;
            _awardConditions = awardConditions;
        }

        [HttpGet("CheckForNewBadges/{userId}")]
        public async Task<ActionResult<List<int>>> CheckForNewBadges(string userId)
        {
            var user = _userRepository.GetUser(userId);
            if (user == null) return BadRequest(new { success = false, message = "User not found" });

            var awardHelper = new AwardHelper(_userBadgeRepository, _badgeRepository);
            List<int> awardedBadges = new List<int>();

            foreach (var awardCondition in _awardConditions)
            {
                int badgeId = _badgeRepository.GetAll().Where(b => b.Name == awardCondition.BadgeName).FirstOrDefault().Id;

                if (await awardHelper.CheckAndAwardAsync(user, awardCondition, badgeId))
                {
                    awardedBadges.Add(badgeId);
                }
            }

            return Ok(awardedBadges);
        }

    }
}