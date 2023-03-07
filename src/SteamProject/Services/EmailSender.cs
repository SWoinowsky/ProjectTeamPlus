using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SteamProject.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;
        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var sendGridKey = configuration["SendGridKey"];
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
                HtmlContent = message,
            };
            msg.AddTo(new EmailAddress(email));
            msg.TemplateId = "d-77f4b1af958a45bbb5745d4df5311754";
            msg.SetTemplateData(new ConfirmEmail { Url = FixUrl(message) });                    
            msg.SetClickTracking(false, false);
            return client.SendEmailAsync(msg);
        }

        private string FixUrl(string url)
        {
            return url.Replace("&amp;", "&");
        }

        private class ConfirmEmail
        {
            [JsonProperty("url")]
            public string Url { get; set; }
        }
    }
}