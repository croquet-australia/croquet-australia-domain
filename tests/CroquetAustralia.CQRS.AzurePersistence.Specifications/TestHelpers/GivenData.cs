using System;
using System.Collections.Generic;
using CroquetAustralia.CQRS.AzurePersistence.Infrastructure;
using Microsoft.WindowsAzure.Storage.Table;

namespace CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers
{
    public class GivenData
    {
        public const string ConnectionString = "UseDevelopmentStorage=true;";

        public IAggregateEvents AggregateEvents;
        public List<IAggregateEvents> AggregateEventsCollection = new List<IAggregateEvents>();
        public Guid AggregateId;
        public IEvent Event;
        public IEventDeserializer EventDeserializer;
        public List<IEvent> Events;
        public IEventSerializer EventSerializer;
        public IEventSerializerRowKeyGenerator EventSerializerRowKeyGenerator;
        public IEventStore EventStore;
        public ITableEntity SerializedEvent;
        public ITableNameResolver TableNameResolver;

        public CloudTable EventsTable<TAggregate>()
        {
            return AzureStorage.GetEventsTable(ConnectionString, TableNameResolver, typeof(TAggregate));
        }
    }
}