using System;
using System.Collections.Generic;

namespace CargoTrack.Services.Identity.API.Domain.Entities
{
    public class User : BaseEntity
    {
        private const int MaxFailedLoginAttempts = 5;
        private const int LockoutDurationMinutes = 30;

        public User() : base()
        {
            Roles = new HashSet<Role>();
            IsActive = true;
        }

        public User(string email, string username, string passwordHash, string firstName, string lastName, string companyName, string phoneNumber) : this()
        {
            Email = email;
            Username = username;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            CompanyName = companyName;
            PhoneNumber = phoneNumber;
        }

        public string Email { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string CompanyName { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool IsActive { get; private set; }
        public bool EmailConfirmed { get; private set; }
        public bool PhoneNumberConfirmed { get; private set; }
        public DateTime? LastLoginDate { get; private set; }
        public int FailedLoginAttempts { get; private set; }
        public DateTime? LockoutEnd { get; private set; }

        public virtual ICollection<Role> Roles { get; private set; }

        public void UpdateProfile(string firstName, string lastName, string companyName, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            CompanyName = companyName;
            PhoneNumber = phoneNumber;
            MarkAsModified();
        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            MarkAsModified();
        }

        public void AddRole(Role role)
        {
            Roles.Add(role);
            MarkAsModified();
        }

        public void RemoveRole(Role role)
        {
            Roles.Remove(role);
            MarkAsModified();
        }

        public void ClearRoles()
        {
            Roles.Clear();
            MarkAsModified();
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
            MarkAsModified();
        }

        public void ConfirmEmail()
        {
            EmailConfirmed = true;
            MarkAsModified();
        }

        public void ConfirmPhoneNumber()
        {
            PhoneNumberConfirmed = true;
            MarkAsModified();
        }

        public void RecordLoginSuccess()
        {
            LastLoginDate = DateTime.UtcNow;
            FailedLoginAttempts = 0;
            LockoutEnd = null;
            MarkAsModified();
        }

        public void RecordLoginFailure()
        {
            FailedLoginAttempts++;
            if (FailedLoginAttempts >= MaxFailedLoginAttempts)
            {
                LockoutEnd = DateTime.UtcNow.AddMinutes(LockoutDurationMinutes);
            }
            MarkAsModified();
        }

        public bool IsLockedOut()
        {
            return LockoutEnd.HasValue && LockoutEnd.Value > DateTime.UtcNow;
        }
    }
} 