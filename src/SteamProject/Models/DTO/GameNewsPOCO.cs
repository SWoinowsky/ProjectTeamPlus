// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
namespace SteamProject.Models.DTO;

public class Appnews
{
    public int appid { get; set; }
    public List<Newsitem> newsitems { get; set; }
    public int count { get; set; }
}

public class Newsitem
{
    public string gid { get; set; }
    public string title { get; set; }
    public string url { get; set; }
    public bool is_external_url { get; set; }
    public string author { get; set; }
    public string contents { get; set; }
    public string feedlabel { get; set; }
    public int date { get; set; }
    public string feedname { get; set; }
    public int feed_type { get; set; }
    public int appid { get; set; }
    public List<string> tags { get; set; }
}

public class GameNewsPoco
{
    public Appnews appnews { get; set; }
}


