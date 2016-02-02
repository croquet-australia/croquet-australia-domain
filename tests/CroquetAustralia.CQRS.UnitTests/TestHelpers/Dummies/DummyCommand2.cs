using System;

namespace CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies
{
    public class DummyCommand2 : CommandBase
    {
        public DummyCommand2(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}