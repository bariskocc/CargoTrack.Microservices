using System;
using System.Collections.Generic;

namespace CargoTrack.Services.Identity.API.Application.DTOs
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public ICollection<string> Permissions { get; set; }
    }

    public class CreateRoleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<string> Permissions { get; set; }
    }

    public class UpdateRoleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class RolePermissionsUpdateDto
    {
        public Guid RoleId { get; set; }
        public ICollection<string> Permissions { get; set; }
    }
} 