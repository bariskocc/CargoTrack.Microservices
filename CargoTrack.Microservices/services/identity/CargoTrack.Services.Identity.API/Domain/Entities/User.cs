using System;
using System.Collections.Generic;

namespace CargoTrack.Services.Identity.API.Domain.Entities
{
    public class User : BaseEntity
    {
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
        public ICollection<UserRole> UserRoles { get; private set; }

        private User() { } // For EF Core

        public User(string email, string username, string passwordHash, string firstName, string lastName, 
            string companyName, string phoneNumber)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            CompanyName = companyName ?? throw new ArgumentNullException(nameof(companyName));
            PhoneNumber = phoneNumber;
            IsActive = true;
            EmailConfirmed = false;
            PhoneNumberConfirmed = false;
            FailedLoginAttempts = 0;
            UserRoles = new List<UserRole>();
        }

        public void UpdateProfile(string firstName, string lastName, string companyName, string phoneNumber)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            CompanyName = companyName ?? throw new ArgumentNullException(nameof(companyName));
            PhoneNumber = phoneNumber;
            MarkAsModified();
        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash ?? throw new ArgumentNullException(nameof(newPasswordHash));
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

        public void Deactivate()
        {
            IsActive = false;
            MarkAsModified();
        }

        public void Activate()
        {
            IsActive = true;
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
            if (FailedLoginAttempts >= 5)
            {
                LockoutEnd = DateTime.UtcNow.AddMinutes(30);
            }
            MarkAsModified();
        }

        public void AddRole(Role role)
        {
            UserRoles ??= new List<UserRole>();
            UserRoles.Add(new UserRole(this, role));
            MarkAsModified();
        }

        public void RemoveRole(Role role)
        {
            if (UserRoles == null) return;
            var userRole = UserRoles.FirstOrDefault(ur => ur.RoleId == role.Id);
            if (userRole != null)
            {
                UserRoles.Remove(userRole);
                MarkAsModified();
            }
        }
    }
} 