using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface IInboxRepository : IRepository<InboxMessage>
{
    List<InboxMessage> GetInboxMessages(int userId);
    // List<InboxMessage> GetMessagesFor(int userId);
    List<InboxMessage> GetMessagesBetween(int fromId, int toId);

}