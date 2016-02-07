using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace CroquetAustralia.CQRS.AzurePersistence.Infrastructure
{
    public class EventSerializer : IEventSerializer
    {
        internal const string EventTypeKey = "__eventType";

        private readonly IEventSerializerRowKeyGenerator _rowKeyGenerator;

        public EventSerializer(IEventSerializerRowKeyGenerator rowKeyGenerator)
        {
            _rowKeyGenerator = rowKeyGenerator;
        }

        public ITableEntity Serialize(Guid aggregateId, IEvent @event)
        {
            if (aggregateId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(aggregateId), "Value cannot be empty.");
            }

            var entity = new DynamicTableEntity(aggregateId.ToString(), _rowKeyGenerator.GenerateRowKey());

            entity.Properties.Add(EventTypeKey, EntityProperty.GeneratePropertyForString(@event.GetType().FullName));

            var eventProperties = @event.GetType().GetProperties();

            foreach (var parameterInfo in @event.GetType().GetConstructors().Single().GetParameters())
            {
                var eventProperty = eventProperties.Single(p => p.Name.Equals(parameterInfo.Name, StringComparison.OrdinalIgnoreCase));
                var value = eventProperty.GetValue(@event);
                var entityProperty = EntityProperty.CreateEntityPropertyFromObject(value);

                entity.Properties.Add(parameterInfo.Name, entityProperty);
            }

            return entity;
        }
    }
}