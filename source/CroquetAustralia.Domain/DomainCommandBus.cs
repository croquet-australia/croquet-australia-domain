using System;
using System.Reflection;
using CroquetAustralia.CQRS;

namespace CroquetAustralia.Domain
{
    public class DomainCommandBus : CommandBus
    {
        public DomainCommandBus(IEventStore eventStore, IServiceProvider commandHandlersProvider)
            : base(Assembly.GetExecutingAssembly(), eventStore, commandHandlersProvider)
        {
        }
    }
}