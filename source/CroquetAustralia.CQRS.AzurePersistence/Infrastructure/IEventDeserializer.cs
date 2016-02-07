using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace CroquetAustralia.CQRS.AzurePersistence.Infrastructure
{
    public interface IEventDeserializer
    {
        EntityResolver<IEvent> Deserializer { get; }
        IEvent Deserialize(IDictionary<string, EntityProperty> entityProperties);
    }
}