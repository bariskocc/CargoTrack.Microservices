using System.Threading.Tasks;

namespace CargoTrack.Services.Identity.API.Domain.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendPasswordResetEmailAsync(string to, string resetToken);
        Task SendEmailVerificationAsync(string to, string verificationToken);
        Task SendAccountLockedNotificationAsync(string to, string reason);
    }
} 