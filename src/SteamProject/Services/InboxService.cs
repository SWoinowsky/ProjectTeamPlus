using SteamProject.Models;
using SteamProject.DAL.Abstract;

namespace SteamProject.Services
{
    public class InboxService : IInboxService
    {
        private readonly IInboxRepository _inboxRepository;
        public InboxService(IInboxRepository inboxRepository)
        {
            _inboxRepository = inboxRepository;
        }

        public void SendToInbox(int userId, string sender , string subject, string content)
        {
            InboxMessage message = new();
            message.RecipientId = userId;
            message.TimeStamp = DateTime.Now;
            message.Sender = sender;
            message.Subject = subject;
            message.Content = content;
            _inboxRepository.AddOrUpdate(message);
        }
        
        public void SendMessage(int toId, int fromId, string content)
        {
            InboxMessage message = new();
            message.RecipientId = toId;
            message.SenderId = fromId;
            message.Content = content;
            message.TimeStamp = DateTime.Now;
            _inboxRepository.AddOrUpdate(message);
        }
    }
}