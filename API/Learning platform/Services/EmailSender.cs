using Learning_platform.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Learning_platform.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderOptions _emailSenderOptions;

        public EmailSender(IOptions<EmailSenderOptions> emailSenderOptions)
        {
            _emailSenderOptions = emailSenderOptions.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient
            {
                Host = _emailSenderOptions.SmtpServer,
                Port = _emailSenderOptions.SmtpPort,
                EnableSsl = _emailSenderOptions.EnableSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSenderOptions.UserName, _emailSenderOptions.Password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSenderOptions.FromAddress),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
