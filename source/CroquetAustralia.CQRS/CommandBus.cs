using System.Reflection;
using System.Threading.Tasks;

namespace CroquetAustralia.CQRS
{
    public class CommandBus : ICommandBus
    {
        private readonly ICommandHandlers _commandHandlers;
        private readonly IEventStore _eventStore;

        public CommandBus(Assembly assembly, ICommandHandlerProvider commandHandlerProvider, IEventStore eventStore)
            : this(new CommandHandlers(assembly, commandHandlerProvider), eventStore)
        {
        }

        public CommandBus(ICommandHandlers commandHandlers, IEventStore eventStore)
        {
            _commandHandlers = commandHandlers;
            _eventStore = eventStore;
        }

        public async Task SendCommandAsync(ICommand command)
        {
            var commandHandler = _commandHandlers.GetCommandHandler(command);
            var events = await commandHandler.Invoke(command);

            await _eventStore.AddEventsAsync(events);
        }
    }
}