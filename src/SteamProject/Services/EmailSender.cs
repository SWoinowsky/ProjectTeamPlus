using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace SteamProject.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var sendGridKey = @"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";
            return Execute(sendGridKey, subject, htmlMessage, email);
        }
        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("steaminfonexus@gmail.com", "SteamNexus"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            msg.TemplateId = "d-f593106acb124af4b36197607cfe611a";
            msg.SetClickTracking(false, false);
            return client.SendEmailAsync(msg);
        }
    }
}