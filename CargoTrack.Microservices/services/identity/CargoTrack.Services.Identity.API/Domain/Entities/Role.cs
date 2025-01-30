using System;
using System.Collections.Generic;

namespace CargoTrack.Services.Identity.API.Domain.Entities
{
    public class Role : BaseEntity
    {
        public Role() : base()
        {
            Users = new HashSet<User>();
            Permissions = new HashSet<Permission>();
        }

        public Role(string name, string description = null) : this()
        {
            Name = name;
            Description = description;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public virtual ICollection<User> Users { get; private set; }
        public virtual ICollection<Permission> Permissions { get; private set; }

        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
            MarkAsModified();
        }

        public void AddPermission(Permission permission)
        {
            Permissions.Add(permission);
            MarkAsModified();
        }

        public void RemovePermission(Permission permission)
        {
            Permissions.Remove(permission);
            MarkAsModified();
        }

        public void ClearPermissions()
        {
            Permissions.Clear();
            MarkAsModified();
        }
    }
} 