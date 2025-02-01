using System;
using MediatR;

namespace CargoTrack.Services.Identity.API.Domain.Events
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
} 