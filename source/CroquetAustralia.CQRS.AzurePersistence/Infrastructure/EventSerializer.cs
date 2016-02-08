using System;
using System.Linq;
using System.Reflection;
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

            try
            {
                var eventType = @event.GetType();
                var entity = new DynamicTableEntity(aggregateId.ToString(), _rowKeyGenerator.GenerateRowKey());

                AddEventMetaDataToTableEntityProperties(entity, eventType);
                AddEventPropertiesToTableEntityProperties(entity, eventType, @event);

                return entity;
            }
            catch (Exception exception)
            {
                throw new EventSerializerException($"Cannot serialize event '{@event.GetType()} / {aggregateId}'.", exception);
            }
        }

        private static void AddEventMetaDataToTableEntityProperties(DynamicTableEntity entity, Type eventType)
        {
            entity.Properties.Add(EventTypeKey, EntityProperty.GeneratePropertyForString(eventType.FullName));
        }

        private static void AddEventPropertiesToTableEntityProperties(DynamicTableEntity entity, Type eventType, IEvent @event)
        {
            var propertyInfos = eventType.GetProperties();
            var constructorInfo = GetEventConstructorInfo(@event);
            var parameterInfos = constructorInfo.GetParameters();

            foreach (var parameterInfo in parameterInfos)
            {
                try
                {
                    var propertyInfo = GetEventProperty(parameterInfo, propertyInfos);
                    var value = GetEventPropertyValue(propertyInfo, @event);
                    var entityProperty = EntityProperty.CreateEntityPropertyFromObject(value);

                    entity.Properties.Add(parameterInfo.Name, entityProperty);
                }
                catch (Exception exception)
                {
                    throw new EventSerializerException($"Cannot add entity property '{parameterInfo.Name}'.", exception);
                }
            }
        }

        private static ConstructorInfo GetEventConstructorInfo(IEvent @event)
        {
            return @event.GetType().GetConstructors().Single();
        }

        private static PropertyInfo GetEventProperty(ParameterInfo parameterInfo, PropertyInfo[] eventProperties)
        {
            return eventProperties.Single(p => p.Name.Equals(parameterInfo.Name, StringComparison.OrdinalIgnoreCase));
        }

        private static object GetEventPropertyValue(PropertyInfo eventProperty, IEvent @event)
        {
            return eventProperty.GetValue(@event);
        }
    }
}