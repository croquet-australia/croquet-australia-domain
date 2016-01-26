using System;

namespace CroquetAustralia.Domain.Specifications.Mocks
{
    internal class AggregateNotFoundException : Exception
    {
        internal AggregateNotFoundException(Type type, Guid aggregateId) : base(CreateMessage(type, aggregateId))
        {
        }

        private static string CreateMessage(Type type, Guid aggregateId)
        {
            return string.Format($"Cannot find aggregate '{type.Name}/{aggregateId}'");
        }
    }
}