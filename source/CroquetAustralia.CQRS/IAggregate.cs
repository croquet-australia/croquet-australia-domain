using System;
using System.Collections.Generic;

namespace CroquetAustralia.CQRS
{
    public interface IAggregate
    {
        Guid Id { get; }
        IEnumerable<IEvent> Events { get; }

        void ApplyEvents(IEnumerable<IEvent> events);
    }
}