using System;
using System.Collections.Generic;
using System.Linq;

namespace CroquetAustralia.CQRS
{
    public static class Events
    {
        public static IEnumerable<IAggregateEvents> For<TAggregate>(Guid aggregateId, params IEvent[] newEvents) where TAggregate : IAggregate
        {
            return new[] {new AggregateEvents<TAggregate>(aggregateId, newEvents)};
        }

        public static IEnumerable<IAggregateEvents> And<TAggregate>(this IEnumerable<IAggregateEvents> aggregateEvents, Guid aggregateId, params IEvent[] newEvents) where TAggregate : IAggregate
        {
            return aggregateEvents.Concat(new[] {new AggregateEvents<TAggregate>(aggregateId, newEvents)});
        }
    }
}