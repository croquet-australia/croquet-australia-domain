using System;

namespace CroquetAustralia.CQRS.AzurePersistence.Infrastructure
{
    public class TableNameResolver : ITableNameResolver
    {
        private readonly string _tableNameFormat;

        public TableNameResolver() : this("{0}Events")
        {
        }

        public TableNameResolver(string tableNameFormat)
        {
            _tableNameFormat = tableNameFormat;
        }

        public string GetTableName(Type aggregateType)
        {
            return string.Format(_tableNameFormat, aggregateType.Name);
        }
    }
}