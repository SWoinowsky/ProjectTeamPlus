// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
namespace SteamProject.Models.DTO;

public class GameInfoPOCO
{
    public GameResponse response {get; set;}
}

public class GameResponse
{
    public Data data { get; set; }
}

public class Category
{
    public int id { get; set; }
    public string description { get; set; }
}

public class Data
{
    public string short_description { get; set; }
    public PcRequirements pc_requirements { get; set; }
    public List<string> developers { get; set; }
    public List<string> publishers { get; set; }
    public List<Demo> demos { get; set; }
    public PriceOverview price_overview { get; set; }
    public Platforms platforms { get; set; }
    public Metacritic metacritic { get; set; }
    public List<Category> categories { get; set; }
    public List<Genre> genres { get; set; }
}

public class Demo
{
    public int appid { get; set; }
    public string description { get; set; }
}

public class Genre
{
    public string id { get; set; }
    public string description { get; set; }
}

public class Metacritic
{
    public int score { get; set; }
}

public class PcRequirements
{
    public string minimum { get; set; }
}

public class Platforms
{
    public bool windows { get; set; }
    public bool mac { get; set; }
    public bool linux { get; set; }
}

public class PriceOverview
{
    public string currency { get; set; }
    public int initial { get; set; }
    public int discount_percent { get; set; }
}


