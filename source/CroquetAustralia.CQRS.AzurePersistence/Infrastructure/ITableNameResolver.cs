using System;

namespace CroquetAustralia.CQRS.AzurePersistence.Infrastructure
{
    public interface ITableNameResolver
    {
        string GetTableName(Type aggregateType);
    }
}