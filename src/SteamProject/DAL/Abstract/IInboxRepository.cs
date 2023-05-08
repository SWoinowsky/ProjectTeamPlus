using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface IInboxRepository : IRepository<InboxMessage>
{
    List<InboxMessage> GetInboxMessages(int userId);
}