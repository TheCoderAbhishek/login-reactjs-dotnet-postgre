namespace dot_net_app.Interface.EmailServiceInterface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string name, string to, string subject, string body);
    }
}
