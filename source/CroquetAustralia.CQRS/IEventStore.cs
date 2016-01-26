using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CroquetAustralia.CQRS
{
    public interface IEventStore
    {
        Task AddEventsAsync(IEnumerable<IAggregateEvents> aggregateEventsCollection);
        Task<TAggregate> GetAggregateAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate, new();
        Task<IEnumerable<TAggregate>> GetAggregatesAsync<TAggregate>() where TAggregate : IAggregate, new();
    }
}