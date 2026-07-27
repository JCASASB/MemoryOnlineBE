using MemoryOnline.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOnline.Domain.Domain
{
    abstract public class DomainUseCaseBase
    {
        protected List<DomainEvent> _events = new List<DomainEvent>();

        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _events.Add(domainEvent);
        }

        public IEnumerable<DomainEvent> GetEvents()
        {
            return _events;
        }
    }
}
