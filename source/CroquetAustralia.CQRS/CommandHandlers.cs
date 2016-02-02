using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace CroquetAustralia.CQRS
{
    internal class CommandHandlers : ICommandHandlers
    {
        private readonly Dictionary<Type, Func<ICommand, Task<IEnumerable<IAggregateEvents>>>> _commandHandlers;

        internal CommandHandlers(Assembly assembly, ICommandHandlerProvider commandHandlerProvider)
            : this(assembly.FindCommandHandlers(commandHandlerProvider))
        {
        }

        internal CommandHandlers(Dictionary<Type, Func<ICommand, Task<IEnumerable<IAggregateEvents>>>> commandHandlers)
        {
            _commandHandlers = commandHandlers;
        }

        public Func<ICommand, Task<IEnumerable<IAggregateEvents>>> GetCommandHandler(ICommand command)
        {
            Func<ICommand, Task<IEnumerable<IAggregateEvents>>> commandHander;

            if (_commandHandlers.TryGetValue(command.GetType(), out commandHander))
            {
                return commandHander;
            }

            throw new CommandHandlerNotFoundException(command);
        }
    }
}