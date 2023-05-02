using Microsoft.AspNetCore.Mvc;
using SteamProject.Models;

namespace SteamProject.Services
{
    public interface IInboxService
    {
        public void SendToInbox(int userId, string sender, string subject, string content);
    }
}
