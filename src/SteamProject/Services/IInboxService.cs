using Microsoft.AspNetCore.Mvc;
using SteamProject.Models;

namespace SteamProject.Services
{
    public interface IInboxService
    {
        public void SendToInbox(User user, string subject, string content);
    }
}
