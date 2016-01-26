using System;
using System.Reflection;

namespace CroquetAustralia.CQRS
{
    public class HandleCommandException : Exception
    {
        public HandleCommandException(ICommand command, Type commandHandlerType, MethodInfo handleCommandMethod, Exception exception)
            : base(CreateMessage(command, commandHandlerType, handleCommandMethod, exception), exception)
        {
        }

        private static string CreateMessage(ICommand command, Type commandHandlerType, MethodInfo handleCommandMethod, Exception exception)
        {
            var method = $"{commandHandlerType.Name}.{handleCommandMethod.Name}({command.GetType().Name} command)";
            var targetInvocationException = exception as TargetInvocationException;

            if (targetInvocationException == null)
            {
                return $"{method} threw exception '{exception.Message}'.";
            }

            if (targetInvocationException.InnerException is NotImplementedException)
            {
                return $"{method} is not implemented.";
            }

            return $"{method} threw exception '{targetInvocationException.InnerException.Message}'.";
        }
    }
}