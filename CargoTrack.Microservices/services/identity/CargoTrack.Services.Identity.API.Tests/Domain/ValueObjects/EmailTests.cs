using System;
using CargoTrack.Services.Identity.API.Domain.ValueObjects;
using Xunit;

namespace CargoTrack.Services.Identity.API.Tests.Domain.ValueObjects
{
    public class EmailTests
    {
        [Theory]
        [InlineData("test@example.com")]
        [InlineData("user.name@domain.com")]
        [InlineData("user+tag@domain.co.uk")]
        public void Create_WithValidEmail_ShouldCreateEmail(string validEmail)
        {
            // Act
            var email = Email.Create(validEmail);

            // Assert
            Assert.NotNull(email);
            Assert.Equal(validEmail.ToLowerInvariant(), email.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Create_WithNullOrEmptyEmail_ShouldThrowArgumentException(string invalidEmail)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Email.Create(invalidEmail));
            Assert.Equal("E-posta adresi boş olamaz.", exception.Message);
        }

        [Theory]
        [InlineData("notanemail")]
        [InlineData("@nodomain")]
        [InlineData("noat.com")]
        [InlineData("spaces in@email.com")]
        public void Create_WithInvalidEmail_ShouldThrowArgumentException(string invalidEmail)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Email.Create(invalidEmail));
            Assert.Equal("Geçersiz e-posta formatı.", exception.Message);
        }

        [Fact]
        public void Equals_WithSameEmail_ShouldReturnTrue()
        {
            // Arrange
            var email1 = Email.Create("test@example.com");
            var email2 = Email.Create("test@example.com");

            // Act & Assert
            Assert.True(email1.Equals(email2));
            Assert.True(email1.Equals((object)email2));
        }

        [Fact]
        public void Equals_WithDifferentEmail_ShouldReturnFalse()
        {
            // Arrange
            var email1 = Email.Create("test1@example.com");
            var email2 = Email.Create("test2@example.com");

            // Act & Assert
            Assert.False(email1.Equals(email2));
            Assert.False(email1.Equals((object)email2));
        }

        [Fact]
        public void Equals_WithNull_ShouldReturnFalse()
        {
            // Arrange
            var email = Email.Create("test@example.com");

            // Act & Assert
            Assert.False(email.Equals(null));
            Assert.False(email.Equals((object)null));
        }

        [Fact]
        public void ImplicitOperator_ShouldReturnEmailValue()
        {
            // Arrange
            var emailObj = Email.Create("test@example.com");

            // Act
            string emailString = emailObj;

            // Assert
            Assert.Equal("test@example.com", emailString);
        }
    }
} 