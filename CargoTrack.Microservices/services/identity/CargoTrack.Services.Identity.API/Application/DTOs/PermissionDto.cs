using System;

namespace CargoTrack.Services.Identity.API.Application.DTOs
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class CreatePermissionDto
    {
        public string Name { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }

    public class UpdatePermissionDto
    {
        public string Name { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
} 