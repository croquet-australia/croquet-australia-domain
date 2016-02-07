using System;
using CroquetAustralia.CQRS.AzurePersistence.Infrastructure;
using CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers;
using TechTalk.SpecFlow;

namespace CroquetAustralia.CQRS.AzurePersistence.Specifications.Steps.Infrastructure
{
    [Binding]
    public class EventSerializerRowKeyGeneratorSteps
    {
        private readonly ActualData _actual;
        private readonly GivenData _given;

        public EventSerializerRowKeyGeneratorSteps(TestContext testContext)
            : this(testContext.Given, testContext.Actual)
        {
        }

        private EventSerializerRowKeyGeneratorSteps(GivenData given, ActualData actual)
        {
            _given = given;
            _actual = actual;
        }

        [Given(@"new EventSerializerRowKeyGenerator\((.*), (.*)\)")]
        public void GivenNewEventSerializerRowKeyGenerator(long ticks, string guid)
        {
            var clock = new Clock(() => new DateTime(ticks));
            var guidFactory = new GuidFactory(() => Guid.Parse(guid));

            _given.EventSerializerRowKeyGenerator = new EventSerializerRowKeyGenerator(clock, guidFactory);
        }

        [When(@"I call EventSerializerRowKeyGenerator\.GenerateRowKey\(\)")]
        public void WhenICallEventSerializerRowKeyGenerator_GenerateRowKey()
        {
            _actual.Result = _given.EventSerializerRowKeyGenerator.GenerateRowKey();
        }
    }
}