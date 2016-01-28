using System;

namespace CroquetAustralia.Domain.Exceptions
{
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException(Type type, Guid aggregateId) : base(CreateMessage(type, aggregateId))
        {
        }

        private static string CreateMessage(Type type, Guid aggregateId)
        {
            return string.Format($"Cannot find aggregate '{type.Name}/{aggregateId}'");
        }
    }
}