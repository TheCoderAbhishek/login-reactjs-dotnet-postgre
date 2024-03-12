using dot_net_app.Interface.EmailServiceInterface;
using dot_net_app.Model.EmailsService;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace dot_net_app.Service.EmailsService
{
    public class EmailsService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailsService(SmtpSettings smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }

        public async Task SendEmailAsync(string name, string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
            message.To.Add(new MailboxAddress(name, to));
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSettings.SenderEmail, _smtpSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
