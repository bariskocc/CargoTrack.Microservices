using System;
using CargoTrack.Services.Identity.API.Infrastructure.Services;
using Xunit;

namespace CargoTrack.Services.Identity.API.Tests.Domain.Services
{
    public class PasswordHasherTests
    {
        private readonly PasswordHasher _passwordHasher;

        public PasswordHasherTests()
        {
            _passwordHasher = new PasswordHasher();
        }

        [Fact]
        public void HashPassword_WithValidPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            var password = "SecureP@ssw0rd";

            // Act
            var hashedPassword = _passwordHasher.HashPassword(password);

            // Assert
            Assert.NotNull(hashedPassword);
            Assert.NotEqual(password, hashedPassword);
        }

        [Theory]
        [InlineData(null)]
        public void HashPassword_WithNullPassword_ShouldThrowArgumentNullException(string password)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _passwordHasher.HashPassword(password));
        }

        [Fact]
        public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
        {
            // Arrange
            var password = "SecureP@ssw0rd";
            var hashedPassword = _passwordHasher.HashPassword(password);

            // Act
            var result = _passwordHasher.VerifyPassword(password, hashedPassword);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
        {
            // Arrange
            var password = "SecureP@ssw0rd";
            var hashedPassword = _passwordHasher.HashPassword(password);

            // Act
            var result = _passwordHasher.VerifyPassword("WrongP@ssw0rd", hashedPassword);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(null, "hashedPassword")]
        [InlineData("password", null)]
        public void VerifyPassword_WithNullValues_ShouldThrowArgumentNullException(string password, string hashedPassword)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _passwordHasher.VerifyPassword(password, hashedPassword));
        }

        [Fact]
        public void VerifyPassword_WithInvalidHashFormat_ShouldReturnFalse()
        {
            // Arrange
            var password = "SecureP@ssw0rd";
            var invalidHash = "invalidhashformat";

            // Act
            var result = _passwordHasher.VerifyPassword(password, invalidHash);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void NeedsRehash_WithCurrentHashFormat_ShouldReturnFalse()
        {
            // Arrange
            var password = "SecureP@ssw0rd";
            var hashedPassword = _passwordHasher.HashPassword(password);

            // Act
            var result = _passwordHasher.NeedsRehash(hashedPassword);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(null)]
        public void NeedsRehash_WithNullHash_ShouldThrowArgumentNullException(string hashedPassword)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _passwordHasher.NeedsRehash(hashedPassword));
        }

        [Fact]
        public void NeedsRehash_WithInvalidHashFormat_ShouldReturnTrue()
        {
            // Arrange
            var invalidHash = "invalidhashformat";

            // Act
            var result = _passwordHasher.NeedsRehash(invalidHash);

            // Assert
            Assert.True(result);
        }
    }
} 