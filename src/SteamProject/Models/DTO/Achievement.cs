
    public class Achievement
    {
        public string apiname { get; set; }
        public int achieved { get; set; }
        public int unlocktime { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class Playerstats
    {
        public string steamID { get; set; }
        public string gameName { get; set; }
        public List<Achievement> achievements { get; set; }
        public bool success { get; set; }
    }

    public class AchievementRoot
    {
        public Playerstats playerstats { get; set; }
    }