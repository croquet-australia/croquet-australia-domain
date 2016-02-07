using System;
using System.Collections.Generic;

namespace CroquetAustralia.CQRS
{
    public static class Aggregate
    {
        public static TAggregate CreateInstance<TAggregate>(IEnumerable<IEvent> events) where TAggregate : IAggregate, new()
        {
            var aggregate = Activator.CreateInstance<TAggregate>();

            return (TAggregate) ApplyEvents(aggregate, events);
        }

        public static object CreateInstance(Type aggregateType, List<IEvent> events)
        {
            var aggregate = (IAggregate) Activator.CreateInstance(aggregateType);

            return ApplyEvents(aggregate, events);
        }

        private static object ApplyEvents(IAggregate aggregate, IEnumerable<IEvent> events)
        {
            aggregate.ApplyEvents(events);

            return aggregate;
        }
    }
}