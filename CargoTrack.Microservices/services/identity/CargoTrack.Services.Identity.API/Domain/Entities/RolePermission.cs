using System;

namespace CargoTrack.Services.Identity.API.Domain.Entities
{
    public class RolePermission : BaseEntity
    {
        public Guid RoleId { get; private set; }
        public Role Role { get; private set; }
        public Guid PermissionId { get; private set; }
        public Permission Permission { get; private set; }

        private RolePermission() { } // For EF Core

        public RolePermission(Role role, Permission permission)
        {
            Role = role ?? throw new ArgumentNullException(nameof(role));
            RoleId = role.Id;
            Permission = permission ?? throw new ArgumentNullException(nameof(permission));
            PermissionId = permission.Id;
        }
    }
} 