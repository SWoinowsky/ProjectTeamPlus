namespace SteamProject.Models.DTO;

public class CompetitionVotePOCO
{
    public int CompetitionId { get; set; }
    public bool WantsToPlayAgain { get; set; }
}

public class GameVotePOCO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int GameId { get; set; }
    public bool WantsToPlay { get; set; }
}
