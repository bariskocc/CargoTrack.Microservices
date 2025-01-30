using System;

namespace CargoTrack.Services.Identity.API.Domain.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
            LastModifiedDate = DateTime.UtcNow;
        }

        public Guid Id { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
        public DateTime LastModifiedDate { get; protected set; }
        public bool IsDeleted { get; protected set; }

        protected void MarkAsModified()
        {
            LastModifiedDate = DateTime.UtcNow;
        }

        public void Delete()
        {
            IsDeleted = true;
            MarkAsModified();
        }
    }
} 