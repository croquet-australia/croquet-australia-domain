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

        public Task<TAggregate> GetAggregateAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate, new()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TAggregate>> GetAggregatesAsync<TAggregate>() where TAggregate : IAggregate, new()
        {
            throw new NotImplementedException();
        }
    }
}