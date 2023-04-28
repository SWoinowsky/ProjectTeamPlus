using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.Models.DTO;

namespace SteamProject.Utilities
{
    public static class SeedBadges
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var badgeRepository = serviceProvider.GetRequiredService<IBadgeRepository>();

            if (!badgeRepository.GetAll().Any())
            {
                var badgeDataJson = File.ReadAllText("Data/badges.json");
                var badgeData = JsonConvert.DeserializeObject<List<BadgeData>>(badgeDataJson);

                foreach (var data in badgeData)
                {
                    string imagePath = $"wwwroot/assets/badges/{data.ImageFileName}";
                    byte[] imageBytes = File.ReadAllBytes(imagePath);

                    var badge = new Badge
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Description = data.Description,
                        Image = imageBytes
                    };

                    badgeRepository.AddOrUpdate(badge);
                }

            }

            await badgeRepository.SaveChangesAsync();
        }
    }
}