using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.WindowsAzure.Storage.Table;

namespace CroquetAustralia.CQRS.AzurePersistence.Infrastructure
{
    public class EventDeserializer : IEventDeserializer
    {
        private readonly IEventTypes _eventTypes;

        public EventDeserializer(IEventTypes eventTypes)
        {
            _eventTypes = eventTypes;
        }

        public EntityResolver<IEvent> Deserializer => Deserialize;

        public IEvent Deserialize(IDictionary<string, EntityProperty> properties)
        {
            var eventType = properties[EventSerializer.EventTypeKey].StringValue;
            var constructorParameters = properties
                .Where(p => p.Key != EventSerializer.EventTypeKey)
                .ToDictionary(p => p.Key, p => p.Value);

            return Deserialize(eventType, constructorParameters);
        }

        private IEvent Deserialize(string partitionKey, string rowKey, DateTimeOffset timestamp, IDictionary<string, EntityProperty> properties, string etag)
        {
            return Deserialize(properties);
        }

        private IEvent Deserialize(string eventTypeName, IDictionary<string, EntityProperty> properties)
        {
            var constructor = _eventTypes.GetConstructorFor(eventTypeName);
            var parameters = constructor
                .GetParameters()
                .Select(p => GetParameterValue(p, properties[p.Name]))
                .ToArray();

            var obj = constructor.Invoke(parameters);
            var @event = (IEvent) obj;

            return @event;
        }

        private static object GetParameterValue(ParameterInfo parameterInfo, EntityProperty entityProperty)
        {
            try
            {
                return Convert.ChangeType(entityProperty.PropertyAsObject, parameterInfo.ParameterType);
            }
            catch (Exception)
            {
                throw new Exception($"Cannot convert EntityProperty.PropertyType '{entityProperty.PropertyType}' to ParameterInfo.ParameterType '{parameterInfo.ParameterType}' for '{parameterInfo.Name}'.");
            }
        }
    }
}