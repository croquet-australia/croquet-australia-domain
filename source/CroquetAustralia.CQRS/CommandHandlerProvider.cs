using System;

namespace CroquetAustralia.CQRS
{
    public class CommandHandlerProvider : ICommandHandlerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandHandlerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }
    }
}