// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
namespace SteamProject.Models.DTO;

public class UserLevelPOCO
{
    public LevelResponse response { get; set; }
}
public class LevelResponse
{
    public int player_level { get; set; }
}

