using System;
using CargoTrack.Services.Identity.API.Domain.Events;
using Xunit;

namespace CargoTrack.Services.Identity.API.Tests.Domain.Events
{
    public class UserEventsTests
    {
        [Fact]
        public void UserCreatedEvent_ShouldInitializeCorrectly()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var username = "testuser";
            var email = "test@example.com";

            // Act
            var @event = new UserCreatedEvent(userId, username, email);

            // Assert
            Assert.Equal(userId, @event.UserId);
            Assert.Equal(username, @event.Username);
            Assert.Equal(email, @event.Email);
            Assert.True(@event.OccurredOn <= DateTime.UtcNow);
        }

        [Fact]
        public void UserLockedOutEvent_ShouldInitializeCorrectly()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var lockoutEnd = DateTime.UtcNow.AddHours(1);

            // Act
            var @event = new UserLockedOutEvent(userId, lockoutEnd);

            // Assert
            Assert.Equal(userId, @event.UserId);
            Assert.Equal(lockoutEnd, @event.LockoutEnd);
            Assert.True(@event.OccurredOn <= DateTime.UtcNow);
        }

        [Fact]
        public void UserRolesUpdatedEvent_ShouldInitializeCorrectly()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var roleNames = new[] { "Admin", "User" };

            // Act
            var @event = new UserRolesUpdatedEvent(userId, roleNames);

            // Assert
            Assert.Equal(userId, @event.UserId);
            Assert.Equal(roleNames, @event.RoleNames);
            Assert.True(@event.OccurredOn <= DateTime.UtcNow);
        }
    }
} 