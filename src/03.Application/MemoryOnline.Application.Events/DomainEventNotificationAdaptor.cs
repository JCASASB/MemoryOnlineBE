using MediatR;
using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Application.Application.Events
{
    public class DomainEventNotificationAdaptor<TDomainEvent> : INotification where TDomainEvent : DomainEvent
    {
        public TDomainEvent DomainEvent { get; }

        public DomainEventNotificationAdaptor(TDomainEvent domainEvent)
        {
            DomainEvent = domainEvent;
        }
    }
}