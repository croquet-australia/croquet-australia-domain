using System;
using CroquetAustralia.CQRS;

namespace CroquetAustralia.Domain.App
{
    public class RanSetup : IEvent
    {
        public RanSetup(Guid aggregateId, string emailAddress)
        {
            AggregateId = aggregateId;
            EmailAddress = emailAddress;
        }

        public Guid AggregateId { get; }
        public string EmailAddress { get; }
    }
}