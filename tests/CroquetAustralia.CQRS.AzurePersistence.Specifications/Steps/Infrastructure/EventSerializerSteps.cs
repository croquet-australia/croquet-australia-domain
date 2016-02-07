using System.Linq;
using CroquetAustralia.CQRS.AzurePersistence.Infrastructure;
using CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage.Table;
using TechTalk.SpecFlow;

namespace CroquetAustralia.CQRS.AzurePersistence.Specifications.Steps.Infrastructure
{
    [Binding]
    public class EventSerializerSteps
    {
        private const string DummyRowKey = "dummy row key";

        private readonly ActualData _actual;
        private readonly GivenData _given;

        public EventSerializerSteps(TestContext testContext)
            : this(testContext.Given, testContext.Actual)
        {
        }

        private EventSerializerSteps(GivenData given, ActualData actual)
        {
            _given = given;
            _actual = actual;

            _given.EventSerializerRowKeyGenerator = A.Fake<IEventSerializerRowKeyGenerator>();
            _given.EventSerializer = new EventSerializer(_given.EventSerializerRowKeyGenerator);

            A.CallTo(() => _given.EventSerializerRowKeyGenerator.GenerateRowKey()).Returns(DummyRowKey);
        }

        [When(@"I call EventSerializer\.Serialize\(aggregateId, event\)")]
        public void WhenICallEventSerializer_SerializeAggregateIdEvent()
        {
            _actual.TryCatch(() => _actual.DynamicTableEntity = (DynamicTableEntity) _given.EventSerializer.Serialize(_given.AggregateId, _given.Event));
        }

        [Scope(Feature = "EventSerializer")]
        [Then(@"the result should be a DynamicTableEntity")]
        public void ThenTheResultShouldBeADynamicTableEntity()
        {
            _actual.DynamicTableEntity.Should().NotBeNull();
        }

        [Then(@"PartitionKey should be aggregateId")]
        public void ThenPartitionKeyShouldBeAggregateId()
        {
            _actual.DynamicTableEntity.PartitionKey.Should().Be(_given.AggregateId.ToString());
        }

        [Then(@"RowKey should be sortable and unique")]
        public void ThenRowKeyShouldBeSortableAndUnique()
        {
            _actual.DynamicTableEntity.RowKey.Should().Be(DummyRowKey);
        }

        [Then(@"there should be DynamicTableEntity property (__[A-Za-z]+$)")]
        public void ThenThereShouldBeDynamicTableEntityProperty(string propertyName)
        {
            _actual.DynamicTableEntity.Properties.Keys.Should().Contain(propertyName);
        }

        [Then(@"DynamicTableEntity\[(.*)] should be event type")]
        public void ThenDynamicTableEntityEventTypeShouldBe(string propertyName)
        {
            _actual.DynamicTableEntity.Properties[propertyName].StringValue.Should().Be(_given.Event.GetType().FullName);
        }

        [Then(@"there should be DynamicTableEntity property for each Event property")]
        public void ThenThereShouldBeDynamicTableEntityPropertyForEachDynamicTableEntityProperty()
        {
            var eventProperties = _given.Event.GetType().GetConstructors().Single().GetParameters().Select(p => p.Name);
            var expectedProperties = eventProperties.Concat(new[] {"__eventType"});

            _actual.DynamicTableEntity.Properties.Keys.ShouldAllBeEquivalentTo(expectedProperties);
        }
    }
}