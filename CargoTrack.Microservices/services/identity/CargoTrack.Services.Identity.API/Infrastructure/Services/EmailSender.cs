using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace CargoTrack.Services.Identity.API.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailSender(IConfiguration configuration)
        {
            _smtpServer = configuration["Email:SmtpServer"];
            _smtpPort = int.Parse(configuration["Email:SmtpPort"]);
            _smtpUsername = configuration["Email:Username"];
            _smtpPassword = configuration["Email:Password"];
            _fromEmail = configuration["Email:FromEmail"];
            _fromName = configuration["Email:FromName"];
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_fromEmail, _fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(to));

            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true
            };

            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"E-posta gönderilirken bir hata oluştu: {ex.Message}", ex);
            }
        }

        public async Task SendPasswordResetEmailAsync(string to, string resetToken)
        {
            var subject = "Şifre Sıfırlama Talebi";
            var body = $@"
                <h2>Şifre Sıfırlama</h2>
                <p>Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayın:</p>
                <p><a href='https://cargotrack.com/reset-password?token={resetToken}'>Şifremi Sıfırla</a></p>
                <p>Bu bağlantı 24 saat geçerlidir.</p>
                <p>Eğer bu talebi siz yapmadıysanız, lütfen bu e-postayı dikkate almayın.</p>";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendEmailVerificationAsync(string to, string verificationToken)
        {
            var subject = "E-posta Doğrulama";
            var body = $@"
                <h2>E-posta Doğrulama</h2>
                <p>E-posta adresinizi doğrulamak için aşağıdaki bağlantıya tıklayın:</p>
                <p><a href='https://cargotrack.com/verify-email?token={verificationToken}'>E-postamı Doğrula</a></p>
                <p>Bu bağlantı 24 saat geçerlidir.</p>";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendAccountLockedNotificationAsync(string to, string reason)
        {
            var subject = "Hesap Kilitleme Bildirimi";
            var body = $@"
                <h2>Hesabınız Kilitlendi</h2>
                <p>Güvenlik nedeniyle hesabınız kilitlendi.</p>
                <p>Sebep: {reason}</p>
                <p>Hesabınızın kilidini kaldırmak için lütfen müşteri hizmetleriyle iletişime geçin.</p>";

            await SendEmailAsync(to, subject, body);
        }
    }
} 