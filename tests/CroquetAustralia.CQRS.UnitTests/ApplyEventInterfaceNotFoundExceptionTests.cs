using CroquetAustralia.CQRS.UnitTests.TestHelpers;
using CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.CQRS.UnitTests
{
    public class ApplyEventInterfaceNotFoundExceptionTests : TestBase
    {
        public class Constructor : ApplyEventInterfaceNotFoundExceptionTests
        {
            [Fact]
            public void ShouldCreateException()
            {
                // Given
                var aggregate = new DummyAggregate();
                var @event = new DummyEvent3();

                // When
                var exception = new ApplyEventInterfaceNotFoundException(aggregate, @event);

                // Then
                exception.Message.Should().Be("Cannot find 'IApplyEvent<DummyEvent3>' on 'CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies.DummyAggregate'.");
            }
        }
    }
}