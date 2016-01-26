using System;
using CroquetAustralia.CQRS;

namespace CroquetAustralia.Domain.Users
{
    public class RegisterUser : CommandBase
    {
        public RegisterUser(Guid aggregateId, string emailAddress) : base(aggregateId)
        {
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; }
    }
}