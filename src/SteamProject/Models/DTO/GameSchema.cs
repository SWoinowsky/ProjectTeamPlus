    public class SchemaAchievement
    {
        public string name { get; set; }
        public int defaultvalue { get; set; }
        public string displayName { get; set; }
        public int hidden { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public string icongray { get; set; }
    }

    public class AvailableGameStats
    {
        public List<SchemaAchievement> achievements { get; set; }
    }

    public class GameSchema
    {
        public string gameName { get; set; }
        public string gameVersion { get; set; }
        public AvailableGameStats availableGameStats { get; set; }
    }

    public class SchemaRoot
    {
        public GameSchema game { get; set; }
    }