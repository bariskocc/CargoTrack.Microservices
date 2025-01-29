using System;
using System.Collections.Generic;

namespace CargoTrack.Services.Identity.API.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public ICollection<string> Roles { get; set; }
        public ICollection<string> Permissions { get; set; }
    }

    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<string> Roles { get; set; }
    }

    public class UpdateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserChangePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
} 