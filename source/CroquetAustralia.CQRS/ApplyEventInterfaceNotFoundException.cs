using System;

namespace CroquetAustralia.CQRS
{
    public class ApplyEventInterfaceNotFoundException : Exception
    {
        public ApplyEventInterfaceNotFoundException(AggregateBase aggregate, IEvent @event) : base(CreateMessage(aggregate, @event))
        {
        }

        private static string CreateMessage(AggregateBase aggregate, IEvent @event)
        {
            return $"Cannot find 'IApplyEvent<{@event.GetType().Name}>' on '{aggregate}'.";
        }
    }
}