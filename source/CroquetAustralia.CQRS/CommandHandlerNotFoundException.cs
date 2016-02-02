using System;

namespace CroquetAustralia.CQRS
{
    public class CommandHandlerNotFoundException : Exception
    {
        public CommandHandlerNotFoundException(ICommand command) : base(CreateMessage(command))
        {
        }

        private static string CreateMessage(ICommand command)
        {
            return $"Cannot find command handler for '{command}'.";
        }
    }
}