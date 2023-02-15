namespace SteamProject.Models.DTO 
{
    #nullable disable
    public class SteamGameDTO
    {
        public class SteamGame
        {
            public int appid {get; set;}
            public string name {get; set;}
            public int playtime_forever {get; set;}
            public string img_icon_url {get; set;}
            public long rtime_last_played {get; set;}
        }
    }
}