using System;
using System.Collections.Generic;

namespace CargoTrack.Services.Identity.API.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string SystemName { get; private set; }
        public string Category { get; private set; }
        public ICollection<RolePermission> RolePermissions { get; private set; }

        private Permission() { } // For EF Core

        public Permission(string name, string description, string systemName, string category)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            SystemName = systemName ?? throw new ArgumentNullException(nameof(systemName));
            Category = category ?? throw new ArgumentNullException(nameof(category));
            RolePermissions = new List<RolePermission>();
        }

        public void UpdateDetails(string name, string description, string category)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Category = category ?? throw new ArgumentNullException(nameof(category));
            MarkAsModified();
        }
    }
} 