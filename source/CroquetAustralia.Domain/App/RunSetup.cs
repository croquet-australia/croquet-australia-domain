using System;
using CroquetAustralia.CQRS;

namespace CroquetAustralia.Domain.App
{
    public class RunSetup : CommandBase
    {
        public RunSetup(Guid aggregateId, string initialAdministratorEmailAddress) : base(aggregateId)
        {
            InitialAdministratorEmailAddress = initialAdministratorEmailAddress;
        }

        public string InitialAdministratorEmailAddress { get; }
    }
}