
using SteamProject.Models;

namespace SteamProject.ViewModels;

public class CompeteVM
{
    public List<Game> SharedGames { get; set; }
    public Friend Friend { get; set; }
    public int Id { get; set; }
    public string SteamId { get; set; }

    public CompeteVM( List<Game> games, Friend friend ) 
    {
        SharedGames = games;
        Friend = friend;
    }
}