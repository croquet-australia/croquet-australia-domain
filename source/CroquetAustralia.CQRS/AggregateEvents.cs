using System;
using System.Collections.Generic;
using System.Linq;

namespace CroquetAustralia.CQRS
{
    public class AggregateEvents<TAggregate> : AggregateEvents where TAggregate : IAggregate
    {
        public AggregateEvents(Guid aggregateId, IEnumerable<IEvent> newEvents)
            : base(typeof(TAggregate), aggregateId, newEvents)
        {
        }
    }

    public class AggregateEvents : IAggregateEvents
    {
        public AggregateEvents(Type aggregateType, Guid aggregateId, IEnumerable<IEvent> newEvents)
        {
            AggregateType = aggregateType;
            AggregateId = aggregateId;
            Events = newEvents.ToArray();

            if (!Events.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(newEvents), "Value cannot be empty.");
            }
        }

        public Guid AggregateId { get; }
        public Type AggregateType { get; }
        public IEnumerable<IEvent> Events { get; }
    }
}