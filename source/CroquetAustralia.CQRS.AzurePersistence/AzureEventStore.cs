using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CroquetAustralia.CQRS.AzurePersistence.Infrastructure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CroquetAustralia.CQRS.AzurePersistence
{
    public class AzureEventStore : IEventStore
    {
        private readonly CloudTableClient _cloudTableClient;
        private readonly IEventDeserializer _eventDeserializer;
        private readonly IEventSerializer _eventSerializer;
        private readonly ITableNameResolver _tableNameResolver;
        private readonly HashSet<string> _tablesUsed = new HashSet<string>();

        public AzureEventStore(string connectionString, ITableNameResolver tableNameResolver, IEventSerializer eventSerializer, IEventDeserializer eventDeserializer)
        {
            _cloudTableClient = CloudStorageAccount.Parse(connectionString).CreateCloudTableClient();
            _tableNameResolver = tableNameResolver;
            _eventSerializer = eventSerializer;
            _eventDeserializer = eventDeserializer;
        }

        public async Task AddEventsAsync(IEnumerable<IAggregateEvents> aggregateEventsCollection)
        {
            var aggregateEventsArray = aggregateEventsCollection.ToArray();

            if (!aggregateEventsArray.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(aggregateEventsCollection), "Value cannot be empty.");
            }

            var aggregateIdGroups =
                from aggregrateEvents in aggregateEventsArray
                group aggregrateEvents by aggregrateEvents.AggregateId
                into grp
                select grp;

            foreach (var aggregateEvents in aggregateIdGroups.SelectMany(aggregateIdGroup => aggregateIdGroup))
            {
                // 8 Feb 2016
                // The document maximum operations be batch operation is 1,000. However
                // with at least the Storage Emulator the real maximum is 100.
                const int maximumOperationInABatch = 100;
                var taken = 0;
                var events = aggregateEvents.Events.ToArray();

                do
                {
                    var batchOperation = new TableBatchOperation();

                    var eventEntities =
                        from @event in events.Skip(taken).Take(maximumOperationInABatch)
                        select _eventSerializer.Serialize(aggregateEvents.AggregateId, @event);

                    foreach (var eventEntity in eventEntities)
                    {
                        batchOperation.Insert(eventEntity);
                    }

                    var table = await CreateCloudTableAsync(aggregateEvents.AggregateType);
                    await table.ExecuteBatchAsync(batchOperation);

                } while ((taken += maximumOperationInABatch) < events.Length);
            }
        }

        public Task<IEnumerable<IAggregateEvents>> GetAllAsync<TAggregate>() where TAggregate : IAggregate
        {
            return GetAllAsync(typeof(TAggregate));
        }

        public async Task<IEnumerable<IAggregateEvents>> GetAllAsync(Type aggregateType)
        {
            var tableEntities = await GetAllTableEntities(aggregateType);

            var groups =
                from tableEntity in tableEntities
                group tableEntity by tableEntity.PartitionKey
                into grp
                select grp;

            var aggregateEventsCollection =
                from grp in groups
                let aggregateId = Guid.Parse(grp.Key)
                select new AggregateEvents(aggregateType, aggregateId, GetEvents(grp));

            return aggregateEventsCollection;
        }

        public Task<IEnumerable<IEvent>> GetEventsAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            return GetEventsAsync(typeof(TAggregate), aggregateId);
        }

        public async Task<IEnumerable<IEvent>> GetEventsAsync(Type aggregateType, Guid aggregateId)
        {
            TableContinuationToken token = null;

            var table = await CreateCloudTableAsync(aggregateType);
            var query = new TableQuery().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, aggregateId.ToString()));
            var events = new List<IEvent>();

            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync(query, _eventDeserializer.Deserializer, token);

                token = segment.ContinuationToken;
                events.AddRange(segment);
            } while (token != null);

            if (!events.Any())
            {
                throw new AggregateNotFoundException(aggregateType, aggregateId);
            }

            return events;
        }

        private async Task<CloudTable> CreateCloudTableAsync(Type aggregateType)
        {
            var tableName = _tableNameResolver.GetTableName(aggregateType);
            var cloudTable = _cloudTableClient.GetTableReference(tableName);

            // todo: refactor into AzureStorageMagic
            if (_tablesUsed.Contains(tableName))
            {
                return cloudTable;
            }

            _tablesUsed.Add(tableName);
            await cloudTable.CreateIfNotExistsAsync();
            return cloudTable;
        }

        private async Task<IEnumerable<DynamicTableEntity>> GetAllTableEntities(Type aggregateType)
        {
            TableContinuationToken token = null;

            var table = await CreateCloudTableAsync(aggregateType);
            var query = new TableQuery();
            var entities = new List<DynamicTableEntity>();

            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync(query, token);
                token = segment.ContinuationToken;

                entities.AddRange(segment);
            } while (token != null);

            return entities;
        }

        private IEnumerable<IEvent> GetEvents(IEnumerable<DynamicTableEntity> tableEntities)
        {
            return
                from tableEntity in tableEntities
                orderby tableEntity.RowKey
                select _eventDeserializer.Deserialize(tableEntity.Properties);
        }
    }
}