using System;
using CroquetAustralia.CQRS;

namespace CroquetAustralia.Domain.Users
{
    public class RegisteredUser : IEvent
    {
        public RegisteredUser(Guid aggregateId, string emailAddress)
        {
            AggregateId = aggregateId;
            EmailAddress = emailAddress;
        }

        public Guid AggregateId { get; }
        public string EmailAddress { get; }
    }
}