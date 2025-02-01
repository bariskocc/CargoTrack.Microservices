using System;
using System.Text.RegularExpressions;

namespace CargoTrack.Services.Identity.API.Domain.ValueObjects
{
    public class Username
    {
        private const string UsernamePattern = @"^[a-zA-Z0-9_-]{3,20}$";
        public string Value { get; }

        private Username(string value)
        {
            Value = value;
        }

        public static Username Create(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Kullanıcı adı boş olamaz.");

            if (!Regex.IsMatch(username, UsernamePattern))
                throw new ArgumentException("Kullanıcı adı 3-20 karakter uzunluğunda olmalı ve sadece harf, rakam, alt çizgi ve tire içermelidir.");

            return new Username(username.ToLowerInvariant());
        }

        public static implicit operator string(Username username) => username.Value;

        public override string ToString() => Value;

        protected bool Equals(Username other)
        {
            if (other is null) return false;
            return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Username)obj);
        }

        public override int GetHashCode()
        {
            return Value != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Value) : 0;
        }
    }
} 