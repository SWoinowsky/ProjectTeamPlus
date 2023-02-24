namespace SteamProject.Models.DTO;

public class FriendsListPOCO
{
    public Friendslist friendslist { get; set; }
}

public class Friendslist
{
    public List<FriendPOCO> friends { get; set; }
}

public class FriendPOCO
{
    public string steamid { get; set; }
    public string relationship { get; set; }
    public int friend_since { get; set; }
}