using System;
using System.Collections.Generic;
using CargoTrack.Services.Identity.API.Domain.Events;

namespace CargoTrack.Services.Identity.API.Domain.Common
{
    public abstract class AggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents = new();
        
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }

    public abstract class AggregateRoot<TId> : AggregateRoot
    {
        public TId Id { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
        public DateTime LastModifiedDate { get; protected set; }

        protected AggregateRoot()
        {
            CreatedDate = DateTime.UtcNow;
            LastModifiedDate = DateTime.UtcNow;
        }

        protected void UpdateModificationDate()
        {
            LastModifiedDate = DateTime.UtcNow;
        }
    }
} 