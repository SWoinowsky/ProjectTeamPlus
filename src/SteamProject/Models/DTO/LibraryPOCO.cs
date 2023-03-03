namespace SteamProject.Models.DTO;

public class LibraryPOCO
{
    public LibraryResponse response {get; set;}
}
public class Games
    {
        public int appid { get; set; }
        public string name { get; set; }
        public int playtime_forever { get; set; }
        public string img_icon_url { get; set; }
        public int rtime_last_played { get; set; }
    }

    public class LibraryResponse
    {
        public List<Games> games { get; set; }
    }