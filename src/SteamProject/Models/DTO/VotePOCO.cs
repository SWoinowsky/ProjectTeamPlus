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
    public int CompetitionId { get; set; }
}

public class GameVoteDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int AppId { get; set; }
    public int VoteCount { get; set; }
    public bool? CurrentUserVote { get; set; }
}
