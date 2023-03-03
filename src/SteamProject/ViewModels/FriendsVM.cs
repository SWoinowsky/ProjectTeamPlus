
using SteamProject.Models;

namespace SteamProject.ViewModels;

public class FriendsVM
{
    public List<Game> SharedGames { get; set; }
    public Friend Friend { get; set; }
    public int Id { get; set; }
    public string SteamId { get; set; }

    public FriendsVM( List<Game> games, Friend friend ) 
    {
        SharedGames = games;
        Friend = friend;
    }
}