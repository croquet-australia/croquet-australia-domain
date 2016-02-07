using System;
using CroquetAustralia.CQRS.AzurePersistence.Infrastructure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers
{
    internal class AzureStorage
    {
        public static CloudTable GetEventsTable(string connectionString, ITableNameResolver tableNameResolver, Type aggregateType)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var tableName = tableNameResolver.GetTableName(aggregateType);
            var table = tableClient.GetTableReference(tableName);

            return table;
        }
    }
}