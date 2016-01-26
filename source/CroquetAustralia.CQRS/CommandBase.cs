using System;

namespace CroquetAustralia.CQRS
{
    public abstract class CommandBase : ICommand
    {
        protected CommandBase(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        public Guid AggregateId { get; }
    }
}