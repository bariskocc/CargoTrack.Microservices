using System;
using CargoTrack.Services.Identity.API.Domain.ValueObjects;
using Xunit;

namespace CargoTrack.Services.Identity.API.Tests.Domain.ValueObjects
{
    public class UsernameTests
    {
        [Theory]
        [InlineData("john")]
        [InlineData("john_doe")]
        [InlineData("john-doe")]
        [InlineData("john123")]
        public void Create_WithValidUsername_ShouldCreateUsername(string validUsername)
        {
            // Act
            var username = Username.Create(validUsername);

            // Assert
            Assert.NotNull(username);
            Assert.Equal(validUsername.ToLowerInvariant(), username.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Create_WithNullOrEmptyUsername_ShouldThrowArgumentException(string invalidUsername)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Username.Create(invalidUsername));
            Assert.Equal("Kullanıcı adı boş olamaz.", exception.Message);
        }

        [Theory]
        [InlineData("ab")]  // Çok kısa
        [InlineData("verylongusernameexceeding20chars")]  // Çok uzun
        [InlineData("user@name")]  // Geçersiz karakter
        [InlineData("user name")]  // Boşluk içeriyor
        [InlineData("user#name")]  // Geçersiz karakter
        public void Create_WithInvalidUsername_ShouldThrowArgumentException(string invalidUsername)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Username.Create(invalidUsername));
            Assert.Equal("Kullanıcı adı 3-20 karakter uzunluğunda olmalı ve sadece harf, rakam, alt çizgi ve tire içermelidir.", exception.Message);
        }

        [Fact]
        public void Equals_WithSameUsername_ShouldReturnTrue()
        {
            // Arrange
            var username1 = Username.Create("testuser");
            var username2 = Username.Create("testuser");

            // Act & Assert
            Assert.True(username1.Equals(username2));
            Assert.True(username1.Equals((object)username2));
        }

        [Fact]
        public void Equals_WithDifferentUsername_ShouldReturnFalse()
        {
            // Arrange
            var username1 = Username.Create("user1");
            var username2 = Username.Create("user2");

            // Act & Assert
            Assert.False(username1.Equals(username2));
            Assert.False(username1.Equals((object)username2));
        }

        [Fact]
        public void Equals_WithNull_ShouldReturnFalse()
        {
            // Arrange
            var username = Username.Create("testuser");

            // Act & Assert
            Assert.False(username.Equals(null));
            Assert.False(username.Equals((object)null));
        }

        [Fact]
        public void ImplicitOperator_ShouldReturnUsernameValue()
        {
            // Arrange
            var usernameObj = Username.Create("testuser");

            // Act
            string usernameString = usernameObj;

            // Assert
            Assert.Equal("testuser", usernameString);
        }
    }
} 