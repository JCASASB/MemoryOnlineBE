using MemoryOnline.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOnline.Domain.Domain
{
    public interface IEventDomainBase
    {
        IEnumerable<DomainEvent> GetEvents();
    }
}
