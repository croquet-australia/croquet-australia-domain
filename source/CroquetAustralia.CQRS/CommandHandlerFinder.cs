using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CroquetAustralia.CQRS
{
    internal static class CommandHandlerFinder
    {
        private static readonly string HandleCommandAsyncMethodName = GetHandleCommandAsyncMethodName();

        internal static Dictionary<Type, Func<ICommand, Task<IEnumerable<IAggregateEvents>>>> FindCommandHandlers(this Assembly domainAssembly, ICommandHandlerProvider commandHandlerProvider)
        {
            var handlers =
                from t in domainAssembly.GetTypes()
                from i in t.GetInterfaces()
                where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandleCommand<>)
                let commandType = i.GetGenericArguments().Single()
                select new
                {
                    CommandType = commandType,
                    CommandHandler = MakeCommandHandler(t, i, commandHandlerProvider)
                };

            return handlers.ToDictionary(handler => handler.CommandType, handler => handler.CommandHandler);
        }

        private static string GetHandleCommandAsyncMethodName()
        {
            return typeof(IHandleCommand<>).GetMethods().Single().Name;
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

        private static Task<IEnumerable<IAggregateEvents>> HandleCommand(ICommand command, Type commandHandlerType, MethodInfo handleCommandMethod, ICommandHandlerProvider commandHandlerProvider)
        {
            try
            {
                var commandHandler = commandHandlerProvider.GetRequiredService(commandHandlerType);
                var task = handleCommandMethod.Invoke(commandHandler, new object[] {command});

                return (Task<IEnumerable<IAggregateEvents>>) task;
            }
            catch (Exception exception)
            {
                throw new HandleCommandException(command, commandHandlerType, handleCommandMethod, exception);
            }
        }

        private static Func<ICommand, Task<IEnumerable<IAggregateEvents>>> MakeCommandHandler(Type commandHandlerType, Type commandHandlerInterface, ICommandHandlerProvider commandHandlerProvider)
        {
            var handleCommandMethod = GetHandleCommandMethod(commandHandlerInterface);

            return command => HandleCommand(command, commandHandlerType, handleCommandMethod, commandHandlerProvider);
        }
    }
}