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

        public Task AddEventsAsync(IEnumerable<IAggregateEvents> aggregateEventsCollection)
        {
            return Task.Run(() => AddEvents(aggregateEventsCollection));
        }

        public Task<IEnumerable<IAggregateEvents>> GetAllAsync<TAggregate>() where TAggregate : IAggregate
        {
            return GetAllAsync(typeof(TAggregate));
        }

        public Task<IEnumerable<IAggregateEvents>> GetAllAsync(Type aggregateType)
        {
            var aggregateEvents = _aggregatesStore.GetOrAdd(aggregateType, new ConcurrentDictionary<Guid, List<IEvent>>());

            return Task.Run<IEnumerable<IAggregateEvents>>(() => aggregateEvents.Select(pair => new AggregateEvents(aggregateType, pair.Key, pair.Value)));
        }

        public Task<IEnumerable<IEvent>> GetEventsAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            return GetEventsAsync(typeof(TAggregate), aggregateId);
        }

        public Task<IEnumerable<IEvent>> GetEventsAsync(Type aggregateType, Guid aggregateId)
        {
            ConcurrentDictionary<Guid, List<IEvent>> aggregates;
            List<IEvent> events;

            if (!_aggregatesStore.TryGetValue(aggregateType, out aggregates) || !aggregates.TryGetValue(aggregateId, out events))
            {
                throw new AggregateNotFoundException(aggregateType, aggregateId);
            }

            return Task.FromResult(events.AsEnumerable());
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