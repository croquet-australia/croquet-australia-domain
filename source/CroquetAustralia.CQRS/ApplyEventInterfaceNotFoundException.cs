using System;

namespace CroquetAustralia.CQRS
{
    internal class ApplyEventInterfaceNotFoundException : Exception
    {
        public ApplyEventInterfaceNotFoundException(AggregateBase aggregateBase, IEvent @event) : base(CreateMessage(aggregateBase, @event))
        {
        }

        private static string CreateMessage(AggregateBase aggregateBase, IEvent @event)
        {
            return $"Cannot find IApplyEvent<{@event.GetType().Name}> on {aggregateBase.GetType().Name} aggregate.";
        }
    }
}