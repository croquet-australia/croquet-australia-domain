using CroquetAustralia.CQRS.UnitTests.TestHelpers;
using CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.CQRS.UnitTests
{
    public class AggregateBaseTests : TestBase
    {
        public class ApplyEvents : AggregateBaseTests
        {
            [Fact]
            public void ShouldCallApplyEventMethodForEachEventIn_events_Parameter()
            {
                // Given
                var events = new IEvent[]
                {
                    new DummyEvent1(),
                    new DummyEvent2()
                };
                var aggregate = new DummyAggregate();

                // When
                aggregate.ApplyEvents(events);

                // Then
                aggregate.AppliedEvent1.Should().Be(1, "because DummyAggregate.ApplyEvent(DummyEvent1) should have been called by DummyAggregate.ApplyEvents(IEnumerable<IEvent>)");
                aggregate.AppliedEvent2.Should().Be(1, "because DummyAggregate.ApplyEvent(DummyEvent2) should have been called by DummyAggregate.ApplyEvents(IEnumerable<IEvent>)");
            }

            [Fact]
            public void ShouldThrow_ApplyEventInterfaceNotFoundException_IfApplyEventMethodCannotBeForAnEventIn_events_Parameter()
            {
                // Given
                var events = new IEvent[]
                {
                    new DummyEvent3()
                };

                var aggregate = new DummyAggregate();

                // When
                var exception = CatchException(() => aggregate.ApplyEvents(events));

                // Then
                exception.Should().BeOfType<ApplyEventInterfaceNotFoundException>();
            }

            [Fact]
            public void ShouldSet_Events_PropertyWith_events_Parameter()
            {
                // Given
                var events = new IEvent[]
                {
                    new DummyEvent1(),
                    new DummyEvent2()
                };
                var aggregate = new DummyAggregate();

                // When
                aggregate.ApplyEvents(events);

                // Then
                aggregate.Events.ShouldAllBeEquivalentTo(events);
                aggregate.Events.Should().NotBeSameAs(events);
            }
        }
    }
}