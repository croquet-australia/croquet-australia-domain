using System;
using CroquetAustralia.CQRS;

namespace CroquetAustralia.Domain.Users
{
    public class RegisteredUser : IEvent
    {
        public RegisteredUser(RegisterUser command)
        {
            AggregateId = command.AggregateId;
            EmailAddress = command.EmailAddress;
        }

        public Guid AggregateId { get; }
        public string EmailAddress { get; }
    }
}