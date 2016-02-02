using System;
using System.Reflection;
using CroquetAustralia.CQRS;

namespace CroquetAustralia.Domain
{
    public class DomainCommandBus : CommandBus
    {
        public DomainCommandBus(ICommandHandlerProvider commandHandlersProvider, IEventStore eventStore)
            : base(Assembly.GetExecutingAssembly(), commandHandlersProvider, eventStore)
        {
        }
    }
}