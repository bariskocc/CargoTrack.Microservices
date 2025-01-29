using System;
using System.Collections.Generic;

namespace CargoTrack.Services.Identity.API.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ICollection<UserRole> UserRoles { get; private set; }
        public ICollection<RolePermission> RolePermissions { get; private set; }

        private Role() { } // For EF Core

        public Role(string name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            UserRoles = new List<UserRole>();
            RolePermissions = new List<RolePermission>();
        }

        public void UpdateDetails(string name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            MarkAsModified();
        }

        public void AddPermission(Permission permission)
        {
            RolePermissions ??= new List<RolePermission>();
            RolePermissions.Add(new RolePermission(this, permission));
            MarkAsModified();
        }

        public void RemovePermission(Permission permission)
        {
            if (RolePermissions == null) return;
            var rolePermission = RolePermissions.FirstOrDefault(rp => rp.PermissionId == permission.Id);
            if (rolePermission != null)
            {
                RolePermissions.Remove(rolePermission);
                MarkAsModified();
            }
        }
    }
} 