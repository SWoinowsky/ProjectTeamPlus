namespace SteamProject.Services
{
    public interface IInboxService
    {
        public Task SendToInbox(string subject, string content);
    }
}
