using System;
using CroquetAustralia.CQRS;

namespace CroquetAustralia.Domain.Specifications.Helpers
{
    public class GivenData
    {
        public Guid AggregateId;
        public ICommand Command;
        public string EmailAddress;
    }
}