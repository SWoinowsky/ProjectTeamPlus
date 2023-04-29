using SteamProject.Models;

namespace SteamProject.Services
{
    public interface IInboxService
    {
        public string SendToInbox(User user, string subject, string content);
    }
}
