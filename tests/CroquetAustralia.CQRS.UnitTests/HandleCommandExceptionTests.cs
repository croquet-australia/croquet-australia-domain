using System;
using System.Linq;
using System.Reflection;
using CroquetAustralia.CQRS.UnitTests.TestHelpers;
using CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.CQRS.UnitTests
{
    public class HandleCommandExceptionTests : TestBase
    {
        public class Constructor : HandleCommandExceptionTests
        {
            [Fact]
            public void ShouldCreateExceptionWhen_exception_Is_TargetInvocationException()
            {
                // Given
                var command = new DummyCommand1(Guid.NewGuid());
                var commandHandlerType = typeof(DummyCommandHandler);
                var handleCommandMethod = commandHandlerType.GetMethods().First();
                var innerException = new Exception("dummy inner exception");
                var targetInvocationException = new TargetInvocationException("dummy message", innerException);

                // When
                var handleCommandException = new HandleCommandException(command, commandHandlerType, handleCommandMethod, targetInvocationException);

                // Then
                handleCommandException.Message.Should().Be("DummyCommandHandler.HandleCommandAsync(DummyCommand1 command) threw exception 'dummy inner exception'.");
                handleCommandException.InnerException.Should().Be(targetInvocationException);
            }

            [Fact]
            public void ShouldCreateExceptionWhen_exception_Is_TargetInvocationException_AndItsInnerExceptionIs_NotImplementedException()
            {
                // Given
                var command = new DummyCommand1(Guid.NewGuid());
                var commandHandlerType = typeof(DummyCommandHandler);
                var handleCommandMethod = commandHandlerType.GetMethods().First();
                var innerException = new NotImplementedException("not implemented inner exception");
                var targetInvocationException = new TargetInvocationException("dummy message", innerException);

                // When
                var handleCommandException = new HandleCommandException(command, commandHandlerType, handleCommandMethod, targetInvocationException);

                // Then
                handleCommandException.Message.Should().Be("DummyCommandHandler.HandleCommandAsync(DummyCommand1 command) is not implemented.");
                handleCommandException.InnerException.Should().Be(targetInvocationException);
            }

            [Fact]
            public void ShouldCreateExceptionWhen_exception_IsNot_TargetInvocationException()
            {
                // Given
                var command = new DummyCommand1(Guid.NewGuid());
                var commandHandlerType = typeof(DummyCommandHandler);
                var handleCommandMethod = commandHandlerType.GetMethods().First();
                var exception = new Exception("dummy exception message");

                // When
                var handleCommandException = new HandleCommandException(command, commandHandlerType, handleCommandMethod, exception);

                // Then
                handleCommandException.Message.Should().Be("DummyCommandHandler.HandleCommandAsync(DummyCommand1 command) threw exception 'dummy exception message'.");
                handleCommandException.InnerException.Should().Be(exception);
            }
        }
    }
}