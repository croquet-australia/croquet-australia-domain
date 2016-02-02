using System;
using CroquetAustralia.CQRS.UnitTests.TestHelpers;
using CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.CQRS.UnitTests
{
    public class CommandBaseTests : TestBase
    {
        public class Constructor : CommandBaseTests
        {
            [Fact]
            public void ShouldSetProperties()
            {
                // Given
                var aggregateId = Guid.NewGuid();

                // When
                var command = new DummyCommand1(aggregateId);

                // Then
                command.AggregateId.Should().Be(aggregateId);
            }
        }
    }
}