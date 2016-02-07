using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CroquetAustralia.CQRS
{
    public interface IEventStore
    {
        Task AddEventsAsync(IEnumerable<IAggregateEvents> aggregateEventsCollection);

        Task<IEnumerable<IAggregateEvents>> GetAllAsync<TAggregate>() where TAggregate : IAggregate;

        Task<IEnumerable<IAggregateEvents>> GetAllAsync(Type aggregateType);

        Task<IEnumerable<IEvent>> GetEventsAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate;

        Task<IEnumerable<IEvent>> GetEventsAsync(Type aggregateType, Guid aggregateId);
    }
}