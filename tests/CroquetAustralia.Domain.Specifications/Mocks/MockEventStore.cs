using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CroquetAustralia.CQRS;

namespace CroquetAustralia.Domain.Specifications.Mocks
{
    internal class MockEventStore : IEventStore
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, List<IEvent>>> _aggregatesStore;

        public MockEventStore()
        {
            _aggregatesStore = new ConcurrentDictionary<Type, ConcurrentDictionary<Guid, List<IEvent>>>();
        }

        public Task<TAggregate> GetAggregateAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate, new()
        {
            ConcurrentDictionary<Guid, List<IEvent>> aggregates;
            List<IEvent> events;

            if (!_aggregatesStore.TryGetValue(typeof(TAggregate), out aggregates) || !aggregates.TryGetValue(aggregateId, out events))
            {
                throw new AggregateNotFoundException(typeof(TAggregate), aggregateId);
            }

            var aggregate = CreateAggregate<TAggregate>(events);

            return Task.FromResult(aggregate);
        }

        public Task<IEnumerable<TAggregate>> GetAggregatesAsync<TAggregate>() where TAggregate : IAggregate, new()
        {
            var aggregateEvents = _aggregatesStore.GetOrAdd(typeof(TAggregate), new ConcurrentDictionary<Guid, List<IEvent>>());
            var aggregates = aggregateEvents.Select(ae => CreateAggregate<TAggregate>(ae.Value));

            return Task.Run(() => aggregates.AsEnumerable());
        }

        public Task AddEventsAsync(IEnumerable<IAggregateEvents> aggregateEventsCollection)
        {
            return Task.Run(() => AddEvents(aggregateEventsCollection));
        }

        private static TAggregate CreateAggregate<TAggregate>(List<IEvent> events) where TAggregate : IAggregate, new()
        {
            var aggregate = new TAggregate();
            aggregate.ApplyEvents(events);
            return aggregate;
        }

        private void AddEvents(IEnumerable<IAggregateEvents> aggregateEventsCollection)
        {
            foreach (var aggregateEvents in aggregateEventsCollection)
            {
                var aggregates = _aggregatesStore.GetOrAdd(aggregateEvents.AggregateType, new ConcurrentDictionary<Guid, List<IEvent>>());
                var aggregate = aggregates.GetOrAdd(aggregateEvents.AggregateId, new List<IEvent>());

                aggregate.AddRange(aggregateEvents.Events);
            }
        }
    }
}