using System;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.EventHandlers;
using CargoTrack.Services.Identity.API.Domain.Events;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using CargoTrack.Services.Identity.API.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CargoTrack.Services.Identity.API.Tests.Application.EventHandlers
{
    public class UserEventHandlersTests
    {
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<UserCreatedEventHandler>> _userCreatedLoggerMock;
        private readonly Mock<ILogger<UserLockedOutEventHandler>> _userLockedOutLoggerMock;
        private readonly Mock<ILogger<UserRolesUpdatedEventHandler>> _userRolesUpdatedLoggerMock;

        public UserEventHandlersTests()
        {
            _emailSenderMock = new Mock<IEmailSender>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _userCreatedLoggerMock = new Mock<ILogger<UserCreatedEventHandler>>();
            _userLockedOutLoggerMock = new Mock<ILogger<UserLockedOutEventHandler>>();
            _userRolesUpdatedLoggerMock = new Mock<ILogger<UserRolesUpdatedEventHandler>>();
        }

        [Fact]
        public async Task UserCreatedEventHandler_ShouldSendWelcomeEmail()
        {
            // Arrange
            var handler = new UserCreatedEventHandler(_emailSenderMock.Object, _userCreatedLoggerMock.Object);
            var @event = new UserCreatedEvent(Guid.NewGuid(), "testuser", "test@example.com");

            // Act
            await handler.Handle(@event, CancellationToken.None);

            // Assert
            _emailSenderMock.Verify(x => x.SendEmailAsync(
                @event.Email,
                "CargoTrack'e Hoş Geldiniz",
                It.Is<string>(body => body.Contains(@event.Username))), 
                Times.Once);
        }

        /*[Fact]
        public async Task UserLockedOutEventHandler_ShouldSendLockoutNotification()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new Domain.Entities.User
            {
                Id = userId,
                Email = "test@example.com",
                Username = "testuser"
            };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(user);

            var handler = new UserLockedOutEventHandler(
                _emailSenderMock.Object,
                _userRepositoryMock.Object,
                _userLockedOutLoggerMock.Object);

            var lockoutEnd = DateTime.UtcNow.AddHours(1);
            var @event = new UserLockedOutEvent(userId, lockoutEnd);

            // Act
            await handler.Handle(@event, CancellationToken.None);

            // Assert
            _emailSenderMock.Verify(x => x.SendAccountLockedNotificationAsync(
                user.Email,
                It.Is<string>(reason => reason.Contains(lockoutEnd.ToLocalTime().ToString()))),
                Times.Once);
        }

        [Fact]
        public async Task UserLockedOutEventHandler_WhenUserNotFound_ShouldNotSendEmail()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync((Domain.Entities.User)null);

            var handler = new UserLockedOutEventHandler(
                _emailSenderMock.Object,
                _userRepositoryMock.Object,
                _userLockedOutLoggerMock.Object);

            var @event = new UserLockedOutEvent(userId, DateTime.UtcNow.AddHours(1));

            // Act
            await handler.Handle(@event, CancellationToken.None);

            // Assert
            _emailSenderMock.Verify(x => x.SendAccountLockedNotificationAsync(
                It.IsAny<string>(),
                It.IsAny<string>()),
                Times.Never);
        }*/

        [Fact]
        public async Task UserRolesUpdatedEventHandler_ShouldLogRoleChanges()
        {
            // Arrange
            var handler = new UserRolesUpdatedEventHandler(_userRolesUpdatedLoggerMock.Object);
            var userId = Guid.NewGuid();
            var roleNames = new[] { "Admin", "User" };
            var @event = new UserRolesUpdatedEvent(userId, roleNames);

            // Act
            await handler.Handle(@event, CancellationToken.None);

            // Assert
            _userRolesUpdatedLoggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(userId.ToString()) && v.ToString().Contains("Admin, User")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
} 