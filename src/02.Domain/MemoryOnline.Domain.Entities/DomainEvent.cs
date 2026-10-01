using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOnline.Domain.Entities
{
    public abstract class DomainEvent
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    }
}
