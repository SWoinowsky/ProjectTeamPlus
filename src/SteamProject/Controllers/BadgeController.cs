using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Helpers;
using SteamProject.Models;
using SteamProject.Models.DTO;
using SteamProject.Services;
using SteamProject.ViewModels;

namespace SteamProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BadgeController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IBadgeRepository _badgeRepository;
        private readonly IUserBadgeRepository _userBadgeRepository;
        private readonly IEnumerable<IAwardCondition> _awardConditions;
        private readonly IUserRepository _userRepository;

        public BadgeController(UserManager<IdentityUser> userManager, IBadgeRepository badgeRepository, IUserBadgeRepository userBadgeRepository, IEnumerable<IAwardCondition> awardConditions, IUserRepository userRepository)
        {
            _userManager = userManager;
            _badgeRepository = badgeRepository;
            _userBadgeRepository = userBadgeRepository;
            _awardConditions = awardConditions;
            _userRepository = userRepository;
        }

        [HttpGet("CheckForNewBadges")]
        public async Task<ActionResult<List<int>>> CheckForNewBadges()
        {
            string? id = _userManager.GetUserId(User);

            if (id is null)
            {
                return BadRequest(new { success = false, message = "User not found" });
            }
            else
            {
                User user = _userRepository.GetUser(id);
                List<int> awardedBadges = new List<int>();

                foreach (var awardCondition in _awardConditions)
                {
                    // Check if the condition is fulfilled
                    bool isFulfilled = await awardCondition.IsFulfilledAsync(user, _userBadgeRepository);

                    if (isFulfilled)
                    {
                        // Find the badge with the same name as the award condition
                        var badge = _badgeRepository.GetAll().FirstOrDefault(b => b.Name == awardCondition.BadgeName);

                        // Check if the badge is null, and continue to the next iteration if it is
                        if (badge == null)
                        {
                            continue;
                        }

                        // Award the badge to the user
                        UserBadge userBadge = new UserBadge
                        {
                            UserId = user.Id,
                            BadgeId = badge.Id,
                        };

                        _userBadgeRepository.AddOrUpdate(userBadge);

                        awardedBadges.Add(badge.Id);
                    }
                }

                return Ok(awardedBadges);
            }
        }



        [HttpGet("GetBadgeDetails/{badgeId}")]
        public async Task<ActionResult<BadgeData>> GetBadgeDetails(int badgeId)
        {
            var badge = _badgeRepository.FindById(badgeId);
            if (badge == null) return NotFound(new { success = false, message = "Badge not found" });

            var badgeData = new BadgeData
            {
                Id = badge.Id,
                Name = badge.Name,
                Description = badge.Description,
                ImageFileName = $"{badge.Id}.png", // Update this with your preferred image file name format.
                ImageData = badge.Image
            };

            return Ok(badgeData);
        }


        [HttpGet("GetBadgeImage/{badgeId}")]
        public async Task<IActionResult> GetBadgeImage(int badgeId)
        {
            var badge = _badgeRepository.FindById(badgeId);
            if (badge == null) return NotFound(new { success = false, message = "Badge not found" });

            return File(badge.Image, "image/png");
        }
    }
}
