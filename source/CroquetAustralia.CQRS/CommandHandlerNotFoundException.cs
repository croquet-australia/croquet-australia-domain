using System;

namespace CroquetAustralia.CQRS
{
    internal class CommandHandlerNotFoundException : Exception
    {
        internal CommandHandlerNotFoundException(ICommand command) : base(CreateMessage(command))
        {
        }

        private static string CreateMessage(ICommand command)
        {
            return $"Cannot find command handler for '{command.GetType()}'.";
        }
    }
}