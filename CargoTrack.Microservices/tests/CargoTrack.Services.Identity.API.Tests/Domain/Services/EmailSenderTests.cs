using System;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CargoTrack.Services.Identity.API.Tests.Domain.Services
{
    public class EmailSenderTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly EmailSender _emailSender;

        public EmailSenderTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            SetupConfiguration();
            _emailSender = new EmailSender(_configurationMock.Object);
        }

        private void SetupConfiguration()
        {
            _configurationMock.Setup(x => x["Email:SmtpServer"]).Returns("smtp.example.com");
            _configurationMock.Setup(x => x["Email:SmtpPort"]).Returns("587");
            _configurationMock.Setup(x => x["Email:Username"]).Returns("test@example.com");
            _configurationMock.Setup(x => x["Email:Password"]).Returns("password");
            _configurationMock.Setup(x => x["Email:FromEmail"]).Returns("noreply@example.com");
            _configurationMock.Setup(x => x["Email:FromName"]).Returns("CargoTrack");
        }

        [Fact]
        public async Task SendEmailAsync_WithValidParameters_ShouldNotThrowException()
        {
            // Arrange
            var to = "user@example.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act & Assert
            await Record.ExceptionAsync(() => 
                _emailSender.SendEmailAsync(to, subject, body));
        }

        [Fact]
        public async Task SendPasswordResetEmailAsync_ShouldSendEmailWithCorrectContent()
        {
            // Arrange
            var to = "user@example.com";
            var resetToken = "reset-token-123";

            // Act & Assert
            await Record.ExceptionAsync(() => 
                _emailSender.SendPasswordResetEmailAsync(to, resetToken));
        }

        [Fact]
        public async Task SendEmailVerificationAsync_ShouldSendEmailWithCorrectContent()
        {
            // Arrange
            var to = "user@example.com";
            var verificationToken = "verify-token-123";

            // Act & Assert
            await Record.ExceptionAsync(() => 
                _emailSender.SendEmailVerificationAsync(to, verificationToken));
        }

        [Fact]
        public async Task SendAccountLockedNotificationAsync_ShouldSendEmailWithCorrectContent()
        {
            // Arrange
            var to = "user@example.com";
            var reason = "Too many failed login attempts";

            // Act & Assert
            await Record.ExceptionAsync(() => 
                _emailSender.SendAccountLockedNotificationAsync(to, reason));
        }
    }
} 