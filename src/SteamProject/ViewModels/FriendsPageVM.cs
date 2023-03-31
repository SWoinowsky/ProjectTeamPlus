using SteamProject.Models;

namespace SteamProject.ViewModels;

public class FriendsPageVM
{
    public List<Friend> Friends { get; set; }
    public int Id { get; set; }
    public string SteamId { get; set; }
    public FriendsPageVM( List<Friend> friends, int id, string steamId)
    {
        Friends = friends;
        Id = id;
        SteamId = steamId;
    }
}