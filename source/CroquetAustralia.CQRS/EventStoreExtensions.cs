using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CroquetAustralia.CQRS
{
    public static class EventStoreExtensions
    {
        public static async Task<TAggregate> GetAggregateAsync<TAggregate>(this IEventStore eventStore, Guid aggregateId)
            where TAggregate : IAggregate, new()
        {
            var events = await eventStore.GetEventsAsync(typeof(TAggregate), aggregateId);
            var aggregate = Aggregate.CreateInstance<TAggregate>(events);

            return aggregate;
        }

        public static async Task<IEnumerable<TAggregate>> GetAggregatesAsync<TAggregate>(this IEventStore eventStore)
            where TAggregate : IAggregate, new()
        {
            var aggregatesWithEvents = await eventStore.GetAllAsync(typeof(TAggregate));
            var aggregates = aggregatesWithEvents.Select(item => Aggregate.CreateInstance<TAggregate>(item.Events));

            return aggregates;
        }
    }
}