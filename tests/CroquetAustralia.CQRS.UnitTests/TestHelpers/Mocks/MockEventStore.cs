using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CroquetAustralia.CQRS.UnitTests.TestHelpers.Mocks
{
    internal class MockEventStore : IEventStore
    {
        internal List<IAggregateEvents> Events = new List<IAggregateEvents>();

        public Task AddEventsAsync(IEnumerable<IAggregateEvents> aggregateEventsCollection)
        {
            Events.AddRange(aggregateEventsCollection);
            return Task.FromResult(0);
        }

        public Task<IEnumerable<IAggregateEvents>> GetAllAsync<TAggregate>() where TAggregate : IAggregate
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IAggregateEvents>> GetAllAsync(Type aggregateType)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEvent>> GetEventsAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEvent>> GetEventsAsync(Type aggregateType, Guid aggregateId)
        {
            throw new NotImplementedException();
        }
    }
}