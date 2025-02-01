using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CargoTrack.Services.Identity.API.Domain.ValueObjects
{
    public class Password
    {
        private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
        public string HashedValue { get; }
        public string Salt { get; }

        private Password(string hashedValue, string salt)
        {
            HashedValue = hashedValue;
            Salt = salt;
        }

        public static Password Create(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Şifre boş olamaz.");

            if (!Regex.IsMatch(password, PasswordPattern))
                throw new ArgumentException("Şifre en az 8 karakter uzunluğunda olmalı ve en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir.");

            var salt = GenerateSalt();
            var hashedPassword = HashPassword(password, salt);

            return new Password(hashedPassword, salt);
        }

        public static Password CreateFromHash(string hashedPassword, string salt)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword))
                throw new ArgumentException("Hash değeri boş olamaz.");

            if (string.IsNullOrWhiteSpace(salt))
                throw new ArgumentException("Salt değeri boş olamaz.");

            return new Password(hashedPassword, salt);
        }

        public bool Verify(string password)
        {
            var hashedAttempt = HashPassword(password, Salt);
            return HashedValue == hashedAttempt;
        }

        private static string GenerateSalt()
        {
            var saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordWithSalt = password + salt;
                var bytes = Encoding.UTF8.GetBytes(passwordWithSalt);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        protected bool Equals(Password other)
        {
            if (other is null) return false;
            return string.Equals(HashedValue, other.HashedValue) && string.Equals(Salt, other.Salt);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Password)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((HashedValue != null ? HashedValue.GetHashCode() : 0) * 397) ^ (Salt != null ? Salt.GetHashCode() : 0);
            }
        }
    }
} 