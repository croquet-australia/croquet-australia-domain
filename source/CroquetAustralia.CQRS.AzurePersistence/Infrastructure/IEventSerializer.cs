using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace CroquetAustralia.CQRS.AzurePersistence.Infrastructure
{
    public interface IEventSerializer
    {
        ITableEntity Serialize(Guid aggregateId, IEvent @event);
    }
}