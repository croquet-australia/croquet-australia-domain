using System;
using System.Collections.Generic;
using System.Linq;

namespace CroquetAustralia.CQRS
{
    public class AggregateEvents<TAggregate> : AggregateEvents where TAggregate : IAggregate
    {
        public AggregateEvents(Guid aggregateId, IEvent[] newEvents)
            : base(typeof(TAggregate), aggregateId, newEvents)
        {
        }
    }

    public class AggregateEvents : IAggregateEvents
    {
        public AggregateEvents(Type aggregateType, Guid aggregateId, IEvent[] newEvents)
        {
            AggregateType = aggregateType;
            AggregateId = aggregateId;
            Events = newEvents.ToArray();
        }

        public Guid AggregateId { get; }
        public Type AggregateType { get; }
        public IEnumerable<IEvent> Events { get; }
    }
}