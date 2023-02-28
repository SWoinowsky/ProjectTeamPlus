using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface IFriendRepository : IRepository<Friend>
{
    List<Friend> GetFriends(int userId);
}