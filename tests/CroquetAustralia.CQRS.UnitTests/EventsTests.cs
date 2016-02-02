using System;
using System.Linq;
using CroquetAustralia.CQRS.UnitTests.TestHelpers;
using CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.CQRS.UnitTests
{
    public class EventsTests : TestBase
    {
        public class For : EventsTests
        {
            [Fact]
            public void ShouldReturnCollectionOf_AggregateEvents_For_aggregateId_And_events()
            {
                // Given
                var aggregateId = Guid.NewGuid();
                var dummyEvent1 = new DummyEvent1();
                var dummyEvent2 = new DummyEvent2();

                // When
                var aggregateEventsCollection = Events
                    .For<DummyAggregate>(aggregateId, dummyEvent1, dummyEvent2)
                    .ToArray();

                // Then
                aggregateEventsCollection.Length.Should().Be(1);

                var aggregateEvents = aggregateEventsCollection.First();

                aggregateEvents.AggregateId.Should().Be(aggregateId);
                aggregateEvents.AggregateType.Should().Be(typeof(DummyAggregate));
                aggregateEvents.Events.ShouldAllBeEquivalentTo(new IEvent[] {dummyEvent1, dummyEvent2});
            }
        }

        public class And : EventsTests
        {
            [Fact]
            public void ShouldReturnAggregateCollectionOf_AggregateEvents()
            {
                // Given
                var aggregateId = Guid.NewGuid();
                var dummyEvent1 = new DummyEvent1();
                var dummyEvent2 = new DummyEvent2();

                // When
                var aggregateEventsCollection = Events
                    .For<DummyAggregate>(aggregateId, dummyEvent1)
                    .And<DummyAggregate>(aggregateId, dummyEvent2)
                    .ToArray();

                // Then
                aggregateEventsCollection.Length.Should().Be(2);

                var aggregateEvents = aggregateEventsCollection[0];

                aggregateEvents.AggregateId.Should().Be(aggregateId);
                aggregateEvents.AggregateType.Should().Be(typeof(DummyAggregate));
                aggregateEvents.Events.ShouldAllBeEquivalentTo(new IEvent[] {dummyEvent1});

                aggregateEvents = aggregateEventsCollection[1];

                aggregateEvents.AggregateId.Should().Be(aggregateId);
                aggregateEvents.AggregateType.Should().Be(typeof(DummyAggregate));
                aggregateEvents.Events.ShouldAllBeEquivalentTo(new IEvent[] {dummyEvent2});
            }
        }
    }
}