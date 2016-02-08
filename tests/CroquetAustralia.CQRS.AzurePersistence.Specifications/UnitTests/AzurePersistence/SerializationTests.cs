using System;
using System.Linq;
using CroquetAustralia.CQRS.AzurePersistence.Infrastructure;
using CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers.Dummies;
using CroquetAustralia.Domain.App;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage.Table;
using Ninject;
using Xunit;

namespace CroquetAustralia.CQRS.AzurePersistence.Specifications.UnitTests.AzurePersistence
{
    public class SerializationTests : UnitTestBase
    {
        [Fact]
        public void ShouldBeAbleToSerializeAndDeserializeAllDomainEvents()
        {
            // Given
            Kernel.Rebind<IEventTypes>().ToMethod(ctx => EventTypes.FromAssemblyContaining(typeof(Domain.DomainCommandBus)));
            var eventSerializer = Kernel.Get<EventSerializer>();
            var eventDeserializer = Kernel.Get<EventDeserializer>();
            var aggregateId = Guid.NewGuid();
            var iEvent = typeof(IEvent);
            var events =
                from type in typeof(RanSetup).Assembly.GetTypes()
                where iEvent.IsAssignableFrom(type)
                select Dummy.Event(type);

            foreach (var @event in events)
            {
                // When
                var serialized = (DynamicTableEntity)eventSerializer.Serialize(aggregateId, @event);
                var deserialized = eventDeserializer.Deserialize(serialized.Properties);

                // Then
                ((object)deserialized).ShouldBeEquivalentTo(@event, $"because {@event.GetType()} should be serializable");
            }
        }
    }
}
