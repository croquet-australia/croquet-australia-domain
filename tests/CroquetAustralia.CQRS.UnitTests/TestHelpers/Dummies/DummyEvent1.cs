using System;

namespace CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies
{
    public class DummyEvent1 : IEvent
    {
        private readonly Guid _aggregateId;

        public DummyEvent1() : this(Guid.NewGuid())
        {
        }

        public DummyEvent1(Guid aggregateId)
        {
            _aggregateId = aggregateId;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DummyEvent1);
        }

        public override int GetHashCode()
        {
            return _aggregateId.GetHashCode();
        }

        private bool Equals(DummyEvent1 obj)
        {
            return obj?._aggregateId == _aggregateId;
        }
    }
}