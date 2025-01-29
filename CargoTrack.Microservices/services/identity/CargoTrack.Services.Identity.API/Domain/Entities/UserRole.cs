using System;

namespace CargoTrack.Services.Identity.API.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public Guid RoleId { get; private set; }
        public Role Role { get; private set; }

        private UserRole() { } // For EF Core

        public UserRole(User user, Role role)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            UserId = user.Id;
            Role = role ?? throw new ArgumentNullException(nameof(role));
            RoleId = role.Id;
        }
    }
} 