namespace CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies
{
    public class DummyAggregate : AggregateBase,
        IApplyEvent<DummyEvent1>,
        IApplyEvent<DummyEvent2>
    {
        public int AppliedEvent1;
        public int AppliedEvent2;

        public void ApplyEvent(DummyEvent1 @event)
        {
            AppliedEvent1++;
        }

        public void ApplyEvent(DummyEvent2 @event)
        {
            AppliedEvent2++;
        }
    }
}