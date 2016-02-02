using System;
using System.Collections.Generic;

namespace CroquetAustralia.CQRS
{
    public interface IAggregateEvents
    {
        Guid AggregateId { get; }
        Type AggregateType { get; }
        IEnumerable<IEvent> Events { get; }
    }
}