using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CroquetAustralia.CQRS
{
    public class CommandBus : ICommandBus
    {
        private static readonly string HandleCommandAsyncMethodName = GetHandleCommandAsyncMethodName();
        private readonly IServiceProvider _commandHandlerProvider;

        private readonly Lazy<Dictionary<Type, Func<ICommand, Task<IEnumerable<IAggregateEvents>>>>> _commandHandlers;
        private readonly IEventStore _eventStore;

        public CommandBus(Assembly domainAssembly, IEventStore eventStore, IServiceProvider commandHandlerProvider)
        {
            _eventStore = eventStore;
            _commandHandlerProvider = commandHandlerProvider;

            _commandHandlers = new Lazy<Dictionary<Type, Func<ICommand, Task<IEnumerable<IAggregateEvents>>>>>(() => FindCommandHandlers(domainAssembly));
        }

        public async Task SendCommandAsync(ICommand command)
        {
            var commandHandler = GetCommandHandler(command);
            var events = await commandHandler.Invoke(command);

            await _eventStore.AddEventsAsync(events);
        }

        private Func<ICommand, Task<IEnumerable<IAggregateEvents>>> GetCommandHandler(ICommand command)
        {
            Func<ICommand, Task<IEnumerable<IAggregateEvents>>> commandHander;

            if (_commandHandlers.Value.TryGetValue(command.GetType(), out commandHander))
            {
                return commandHander;
            }

            throw new CommandHandlerNotFoundException(command);
        }

        private static string GetHandleCommandAsyncMethodName()
        {
            return typeof(IHandleCommand<>).GetMethods().Single().Name;
        }

        private Dictionary<Type, Func<ICommand, Task<IEnumerable<IAggregateEvents>>>> FindCommandHandlers(Assembly domainAssembly)
        {
            var handlers =
                from t in domainAssembly.GetTypes()
                from i in t.GetInterfaces()
                where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandleCommand<>)
                let commandType = i.GetGenericArguments().Single()
                select new
                {
                    CommandType = commandType,
                    CommandHandler = MakeCommandHandler(t, i)
                };

            return handlers.ToDictionary(handler => handler.CommandType, handler => handler.CommandHandler);
        }

        private Func<ICommand, Task<IEnumerable<IAggregateEvents>>> MakeCommandHandler(Type commandHandlerType, Type commandHandlerInterface)
        {
            var handleCommandMethod = GetHandleCommandMethod(commandHandlerInterface);

            return command => HandleCommand(command, commandHandlerType, handleCommandMethod);
        }

        private static MethodInfo GetHandleCommandMethod(Type commandHandlerInterface)
        {
            var method = commandHandlerInterface.GetMethod(HandleCommandAsyncMethodName);

            if (method == null)
            {
                throw new Exception($"Cannot find {HandleCommandAsyncMethodName} method.");
            }

            return method;
        }

        private Task<IEnumerable<IAggregateEvents>> HandleCommand(ICommand command, Type commandHandlerType, MethodInfo handleCommandMethod)
        {
            try
            {
                var commandHandler = _commandHandlerProvider.GetRequiredService(commandHandlerType);
                var task = handleCommandMethod.Invoke(commandHandler, new object[] {command});

                return (Task<IEnumerable<IAggregateEvents>>) task;
            }
            catch (Exception exception)
            {
                throw new HandleCommandException(command, commandHandlerType, handleCommandMethod, exception);
            }
        }
    }
}