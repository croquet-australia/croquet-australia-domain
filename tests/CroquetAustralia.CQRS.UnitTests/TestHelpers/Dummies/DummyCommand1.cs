using System;

namespace CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies
{
    public class DummyCommand1 : CommandBase
    {
        public DummyCommand1(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}