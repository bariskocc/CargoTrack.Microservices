using System;
using System.Collections.Generic;

namespace CargoTrack.Services.Identity.API.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public Permission() : base()
        {
            Roles = new HashSet<Role>();
        }

        public Permission(string name, string systemName, string description = null, string category = null) : this()
        {
            Name = name;
            SystemName = systemName;
            Description = description;
            Category = category;
        }

        public string Name { get; private set; }
        public string SystemName { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }

        public virtual ICollection<Role> Roles { get; private set; }

        public void Update(string name, string systemName, string description, string category)
        {
            Name = name;
            SystemName = systemName;
            Description = description;
            Category = category;
            MarkAsModified();
        }
    }
} 