using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Events;
using MediatR;

namespace CargoTrack.Services.Identity.API.Domain.Interfaces
{
    public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        Task Handle(TDomainEvent notification, CancellationToken cancellationToken);
    }
} 