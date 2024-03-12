using dot_net_app.Model.AccountModel;

namespace dot_net_app.Model.Shared
{
    public class ResendOtpRequest
    {
        public string? Username { get; set; }
        public User? User { get; set; }
    }
}
