using System;
using System.Collections.Generic;
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
    public class EventDeserializerSteps
    {
        private readonly ActualData _actual;
        private readonly GivenData _given;

        public EventDeserializerSteps(TestContext testContext)
            : this(testContext.Given, testContext.Actual, testContext.EventSerializer, testContext.EventDeserializer)
        {
        }

        private EventDeserializerSteps(GivenData given, ActualData actual, IEventSerializer eventSerializer, IEventDeserializer eventDeserializer)
        {
            _given = given;
            _actual = actual;
            _given.EventSerializer = eventSerializer;
            _given.EventDeserializer = eventDeserializer;
        }

        [Given(@"the event serialized")]
        public void GivenTheEventSerialized()
        {
            _given.SerializedEvent = _given.EventSerializer.Serialize(Guid.NewGuid(), _given.Event);
        }

        [When(@"I call EventDeserializer\.Deserializer")]
        public void WhenICallEventDeserializer_Deserializer()
        {
            var properties = ((DynamicTableEntity) _given.SerializedEvent).Properties;

            _actual.Event = _given.EventDeserializer.Deserializer.Invoke("dummy", "dummy", A.Dummy<DateTimeOffset>(), properties, "dummy");
        }

        [Then(@"the returned event should be equalivent to given event")]
        public void ThenTheReturnedEventShouldBeEqualiventToGivenEvent()
        {
            var actual = _actual.Event.GetType().GetProperties().Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(_actual.Event)));
            var expected = _given.Event.GetType().GetProperties().Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(_given.Event)));

            actual.ShouldAllBeEquivalentTo(expected);
        }
    }
}