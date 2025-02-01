using System;
using System.Text.RegularExpressions;

namespace CargoTrack.Services.Identity.API.Domain.ValueObjects
{
    public class Email
    {
        private const string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("E-posta adresi boş olamaz.");

            if (!Regex.IsMatch(email, EmailPattern))
                throw new ArgumentException("Geçersiz e-posta formatı.");

            return new Email(email.ToLowerInvariant());
        }

        public static implicit operator string(Email email) => email.Value;

        public override string ToString() => Value;

        protected bool Equals(Email other)
        {
            if (other is null) return false;
            return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Email)obj);
        }

        public override int GetHashCode()
        {
            return Value != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Value) : 0;
        }
    }
} 