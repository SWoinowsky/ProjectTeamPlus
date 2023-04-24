namespace SteamProject.Models.DTO;

    public class GlobalAchievement
    {
        public string name { get; set; }
        public double percent { get; set; }
    }

    public class Achievementpercentages
    {
        public List<GlobalAchievement> achievements { get; set; }
    }

    public class GAPRoot
    {
        public Achievementpercentages achievementpercentages { get; set; }
    }

