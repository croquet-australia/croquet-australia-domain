using System;
using CroquetAustralia.CQRS.UnitTests.TestHelpers;
using CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.CQRS.UnitTests
{
    public class CommandHandlerNotFoundExceptionTests : TestBase
    {
        public class Constructor : CommandHandlerNotFoundExceptionTests
        {
            [Fact]
            public void ShouldCreateException()
            {
                // Given
                var command = new DummyCommand1(Guid.NewGuid());

                // When
                var exception = new CommandHandlerNotFoundException(command);

                // Then
                exception.Message.Should().Be("Cannot find command handler for 'CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies.DummyCommand1'.");
            }
        }
    }
}