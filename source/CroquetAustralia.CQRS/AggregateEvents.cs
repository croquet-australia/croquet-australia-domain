using System;
using System.Collections.Generic;
using System.Linq;

namespace CroquetAustralia.CQRS
{
    public class AggregateEvents<TAggregate> : IAggregateEvents where TAggregate : IAggregate
    {
        public AggregateEvents(Guid aggregateId, IEvent[] newEvents)
        {
            AggregateType = typeof(TAggregate);
            AggregateId = aggregateId;
            Events = newEvents.AsEnumerable();
        }

        public Guid AggregateId { get; }
        public Type AggregateType { get; }
        public IEnumerable<IEvent> Events { get; }
    }
}