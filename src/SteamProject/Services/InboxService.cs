using SteamProject.Models;
using SteamProject.DAL.Abstract;

namespace SteamProject.Services
{
    public class InboxService : IInboxService
    {
        private readonly IUserRepository _userRepository;
        public InboxService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void SendToInbox(User user, string subject, string content)
        {
            InboxMessage newMessage = new();
            newMessage.TimeStamp = DateTime.UtcNow;
            newMessage.RecipientId = user.Id;
            newMessage.Sender = "S.I.N";
            newMessage.Subject = subject;
            newMessage.Content = content;
            user.InboxMessages.Add(newMessage);
            _userRepository.AddOrUpdate(user);
        }
    }
}