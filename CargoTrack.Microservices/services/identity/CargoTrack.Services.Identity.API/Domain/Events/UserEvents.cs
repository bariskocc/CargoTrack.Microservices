using System;

namespace CargoTrack.Services.Identity.API.Domain.Events
{
    public class UserCreatedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public string Username { get; }
        public string Email { get; }
        public DateTime OccurredOn { get; }

        public UserCreatedEvent(Guid userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public class UserLockedOutEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public DateTime LockoutEnd { get; }
        public DateTime OccurredOn { get; }

        public UserLockedOutEvent(Guid userId, DateTime lockoutEnd)
        {
            UserId = userId;
            LockoutEnd = lockoutEnd;
            OccurredOn = DateTime.UtcNow;
        }
    }

    public class UserRolesUpdatedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public string[] RoleNames { get; }
        public DateTime OccurredOn { get; }

        public UserRolesUpdatedEvent(Guid userId, string[] roleNames)
        {
            UserId = userId;
            RoleNames = roleNames;
            OccurredOn = DateTime.UtcNow;
        }
    }
} 