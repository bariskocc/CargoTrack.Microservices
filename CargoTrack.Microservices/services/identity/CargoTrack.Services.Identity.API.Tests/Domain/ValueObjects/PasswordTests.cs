using System;
using CargoTrack.Services.Identity.API.Domain.ValueObjects;
using Xunit;

namespace CargoTrack.Services.Identity.API.Tests.Domain.ValueObjects
{
    public class PasswordTests
    {
        [Theory]
        [InlineData("Test123!@#")]
        [InlineData("SecureP@ssw0rd")]
        [InlineData("Complex1ty!")]
        public void Create_WithValidPassword_ShouldCreatePassword(string validPassword)
        {
            // Act
            var password = Password.Create(validPassword);

            // Assert
            Assert.NotNull(password);
            Assert.NotNull(password.HashedValue);
            Assert.NotNull(password.Salt);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Create_WithNullOrEmptyPassword_ShouldThrowArgumentException(string invalidPassword)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Password.Create(invalidPassword));
            Assert.Equal("Şifre boş olamaz.", exception.Message);
        }

        [Theory]
        [InlineData("weak")]
        [InlineData("NoSpecial1")]
        [InlineData("nodigit!")]
        [InlineData("nouppercase1!")]
        [InlineData("NOLOWERCASE1!")]
        public void Create_WithInvalidPassword_ShouldThrowArgumentException(string invalidPassword)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Password.Create(invalidPassword));
            Assert.Equal("Şifre en az 8 karakter uzunluğunda olmalı ve en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir.", exception.Message);
        }

        [Fact]
        public void Verify_WithCorrectPassword_ShouldReturnTrue()
        {
            // Arrange
            var password = "SecureP@ssw0rd";
            var hashedPassword = Password.Create(password);

            // Act
            var isValid = hashedPassword.Verify(password);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Verify_WithIncorrectPassword_ShouldReturnFalse()
        {
            // Arrange
            var password = Password.Create("SecureP@ssw0rd");

            // Act
            var isValid = password.Verify("WrongP@ssw0rd");

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void CreateFromHash_WithValidValues_ShouldCreatePassword()
        {
            // Arrange
            var originalPassword = Password.Create("SecureP@ssw0rd");

            // Act
            var recreatedPassword = Password.CreateFromHash(originalPassword.HashedValue, originalPassword.Salt);

            // Assert
            Assert.NotNull(recreatedPassword);
            Assert.Equal(originalPassword.HashedValue, recreatedPassword.HashedValue);
            Assert.Equal(originalPassword.Salt, recreatedPassword.Salt);
        }

        [Theory]
        [InlineData(null, "salt")]
        [InlineData("hash", null)]
        [InlineData(null, null)]
        public void CreateFromHash_WithInvalidValues_ShouldThrowArgumentException(string hash, string salt)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Password.CreateFromHash(hash, salt));
            Assert.Contains("boş olamaz", exception.Message);
        }
    }
} 