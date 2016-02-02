using System;
using CroquetAustralia.CQRS.UnitTests.TestHelpers;
using CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.CQRS.UnitTests
{
    public class AggregateEventsOfTAggregateTests : TestBase
    {
        public class Constructor : AggregateEventsTests
        {
            [Fact]
            public void ShouldSetProperties()
            {
                // Given
                var aggregateId = Guid.NewGuid();
                var events = new IEvent[] {new DummyEvent1(), new DummyEvent2()};

                // When
                var aggregateEvents = new AggregateEvents<DummyAggregate>(aggregateId, events);

                // Then
                aggregateEvents.AggregateId.Should().Be(aggregateId);
                aggregateEvents.AggregateType.Should().Be(typeof(DummyAggregate));
                aggregateEvents.Events.ShouldAllBeEquivalentTo(events);
                aggregateEvents.Events.Should().NotBeSameAs(events);
            }
        }
    }
}