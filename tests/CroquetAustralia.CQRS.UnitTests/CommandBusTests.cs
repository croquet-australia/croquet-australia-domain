using System;
using System.Reflection;
using CroquetAustralia.CQRS.UnitTests.TestHelpers;
using CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies;
using CroquetAustralia.CQRS.UnitTests.TestHelpers.Mocks;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.CQRS.UnitTests
{
    public class CommandBusTests : TestBase
    {
        private readonly Assembly _domainAssembly;
        private readonly DummyCommandHandlerProvider _domainCommandHandlerProvider;
        private readonly MockEventStore _mockEventStore;

        public CommandBusTests()
        {
            _domainAssembly = Assembly.GetExecutingAssembly();
            _domainCommandHandlerProvider = new DummyCommandHandlerProvider();
            _mockEventStore = new MockEventStore();

            DummyCommandHandler.Reset();
        }

        protected CommandBus CreateCommandBus()
        {
            return new CommandBus(_domainAssembly, _domainCommandHandlerProvider, _mockEventStore);
        }

        public class SendCommandAsync : CommandBusTests
        {
            [Fact]
            public void ShouldSend_command_ParameterToRegisteredCommandHandler()
            {
                // Given
                var command = new DummyCommand1(Guid.NewGuid());
                var commandBus = CreateCommandBus();

                // When
                commandBus.SendCommandAsync(command).Wait();

                // Then
                DummyCommandHandler.HandleDummyCommand1.Should().Be(1);
            }

            [Fact]
            public void ShouldThrow_CommandHandlerNotFoundException_WhenCommandlerHandlerCannotBeFoundFor_command_Parameter()
            {
                var command = new DummyCommand2(Guid.NewGuid());
                var commandBus = CreateCommandBus();

                // When
                var exception = CatchException(() => commandBus.SendCommandAsync(command).Wait());

                // Then
                exception.Should().BeOfType<CommandHandlerNotFoundException>();
            }

            [Fact]
            public void ShouldSendEventsReturnedByCommandHandlerToEventStore()
            {
                // Given
                var command = new DummyCommand1(Guid.NewGuid());
                var commandBus = CreateCommandBus();

                // When
                commandBus.SendCommandAsync(command).Wait();

                // Then
                var expectedAggregateEvents = Events.For<DummyAggregate>(command.AggregateId, new DummyEvent1(command.AggregateId));

                _mockEventStore.Events.ShouldAllBeEquivalentTo(expectedAggregateEvents);
            }
        }
    }
}