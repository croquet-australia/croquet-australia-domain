using System;
using CroquetAustralia.CQRS;

namespace CroquetAustralia.Domain.App
{
    public class RanSetup : IEvent
    {
        public RanSetup(RunSetup command)
        {
            AggregateId = command.AggregateId;
            EmailAddress = command.InitialAdministratorEmailAddress;
        }

        public Guid AggregateId { get; }
        public string EmailAddress { get; }
    }
}