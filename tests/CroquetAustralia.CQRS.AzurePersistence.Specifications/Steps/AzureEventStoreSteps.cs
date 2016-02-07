using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CroquetAustralia.CQRS.AzurePersistence.Infrastructure;
using CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers;
using CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers.Dummies;
using FakeItEasy;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage.Table;
using TechTalk.SpecFlow;

namespace CroquetAustralia.CQRS.AzurePersistence.Specifications.Steps
{
    [Binding]
    public class AzureEventStoreSteps
    {
        private readonly ActualData _actual;
        private readonly GivenData _given;

        public AzureEventStoreSteps(TestContext testContext)
            : this(testContext.Given, testContext.Actual, testContext.EventStore, testContext.TableNameResolver)
        {
        }

        private AzureEventStoreSteps(GivenData given, ActualData actual, IEventStore eventStore, ITableNameResolver tableNameResolver)
        {
            _given = given;
            _actual = actual;
            _given.EventStore = eventStore;
            _given.TableNameResolver = tableNameResolver;
        }

        [Given(@"aggregateEventsCollection is empty")]
        public void GivenAggregateEventsCollectionIsEmpty()
        {
            _given.AggregateEventsCollection = new List<IAggregateEvents>();
        }

        [Given(@"(.*) aggregateEvents with (.*) event")]
        public void GivenAggregateEventsWithEvent(int aggregateEventsCount, int eventsCount)
        {
            while (_given.AggregateEventsCollection.Count < aggregateEventsCount)
            {
                var events = new List<IEvent>();

                while (events.Count < eventsCount)
                {
                    events.Add(DummyEventWithManyProperties.RandomValue());
                }

                var aggregateEvents = new AggregateEvents<DummyAggregate>(Guid.NewGuid(), events);

                _given.AggregateEventsCollection.Add(aggregateEvents);
            }
        }

        [Given(@"an aggregateId with (.*) events is in the event store")]
        public void GivenAnAggregateIdWithEventsIsInTheEventStore(int eventsCount)
        {
            _given.AggregateId = Guid.NewGuid();
            _given.Events = Enumerable.Range(1, eventsCount).Select(i => DummyEventWithManyProperties.RandomValue()).ToList<IEvent>();
            _given.AggregateEvents = new AggregateEvents<DummyAggregate>(_given.AggregateId, _given.Events);
            _given.AggregateEventsCollection.Add(_given.AggregateEvents);

            AddAggregateEventsCollection();
        }

        [Given(@"the event store is empty")]
        public void GivenTheEventStoreIsEmpty()
        {
            // nothing to do!
        }

        [Given(@"the event store has")]
        public void GivenTheEventStoreHas(Table table)
        {
            _given.AggregateEventsCollection = table.Rows.Select(CreateAggregateEventsFromTableRow).ToList();
            _given.EventStore.AddEventsAsync(_given.AggregateEventsCollection).Wait();
        }

        [When(@"I call AzureEventStore\\\.GetAllAsync\\\(\)")]
        public void WhenICallAzureEventStore_GetAllAsync()
        {
            _actual.AggregateEventsCollection = _actual.TryCatch(() => _given.EventStore.GetAllAsync<DummyAggregate>().Result.ToArray());
        }

        [When(@"I call AzureEventStore\.GetEventsAsync\(aggregateId\)")]
        public void WhenICallAzureEventStore_GetEventsAsyncAggregateId()
        {
            _actual.Events = _actual.TryCatch(() => _given.EventStore.GetEventsAsync<DummyAggregate>(_given.AggregateId).Result.ToArray());
        }

        [When(@"I call AzureEventStore\.AddEventsAsync\(aggregateEventsCollection\)")]
        public void WhenICallAzureEventStore_AddEventsAsyncAggregateEventsCollection()
        {
            _actual.TryCatch(AddAggregateEventsCollection);
        }

        [Then(@"(.*) row is added to the aggregate's events table")]
        public void ThenRowIsAddedToTheAggregatesEventsTable(int expectedRowCount)
        {
            var table = _given.EventsTable<DummyAggregate>();
            var tableQuery = new TableQuery<DynamicTableEntity>();
            var rows = table.ExecuteQuery(tableQuery).ToArray();

            rows.Length.Should().Be(expectedRowCount);
        }

        [When(@"I call new AzureEventStore\(connectionString, tableNameResolver, eventSerializer\)")]
        public void WhenICallNewAzureEventStoreConnectionStringTableNameResolverEventSerializer()
        {
            const string connectionString = "UseDevelopmentStorage=true;";
            var tableNameResolver = A.Fake<ITableNameResolver>();
            var eventSerializer = A.Fake<IEventSerializer>();
            var eventDeserializer = A.Fake<IEventDeserializer>();

            _actual.Stopwatch = Stopwatch.StartNew();
            _actual.EventStore = new AzureEventStore(connectionString, tableNameResolver, eventSerializer, eventDeserializer);
            _actual.Stopwatch.Stop();
        }

        [Then(@"the object should be created in less than (.*)ms")]
        public void ThenTheObjectShouldBeCreatedInLessThan(int maximumElapsedMilliseconds)
        {
            _actual.EventStore.Should().NotBeNull();
            _actual.Stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(maximumElapsedMilliseconds);
        }

        [Then(@"(.*) events should be returned")]
        public void ThenEventsShouldBeReturned(int expectedEventsCount)
        {
            _actual.Exception.Should().BeNull();
            _actual.Events.Count().Should().Be(expectedEventsCount);
        }

        [Then(@"an empty dictionary is returned")]
        public void ThenAnEmptyDictionaryIsReturned()
        {
            _actual.Exception.Should().BeNull();
            _actual.AggregateEventsCollection.Should().BeEmpty();
        }

        [Then(@"(.*) IAggregateEvents should be returned")]
        public void ThenIAggregateEventsCollectionShouldBeReturned(int expectedCount)
        {
            _actual.Exception.Should().BeNull();
            _actual.AggregateEventsCollection.Length.Should().Be(expectedCount);
        }

        [Then(@"the number events in each IAggregateEvents should equal given eventCount")]
        public void ThenTheNumberEventsInEachIAggregateEventsShouldEqualGivenEventCount()
        {
            var actual = _actual.AggregateEventsCollection.Select(ae => new KeyValuePair<Guid, int>(ae.AggregateId, ae.Events.Count()));
            var expected = _given.AggregateEventsCollection.Select(ae => new KeyValuePair<Guid, int>(ae.AggregateId, ae.Events.Count()));

            actual.ShouldAllBeEquivalentTo(expected);
        }

        private void AddAggregateEventsCollection()
        {
            _given.EventStore.AddEventsAsync(_given.AggregateEventsCollection).Wait();
        }

        private static IAggregateEvents CreateAggregateEventsFromTableRow(TableRow arg)
        {
            return new AggregateEvents<DummyAggregate>(Guid.NewGuid(), Enumerable.Range(0, int.Parse(arg["eventCount"])).Select(i => DummyEventWithManyProperties.RandomValue()));
        }
    }
}