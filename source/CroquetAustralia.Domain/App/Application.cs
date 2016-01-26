using CroquetAustralia.CQRS;

namespace CroquetAustralia.Domain.App
{
    public class Application : AggregateBase, IApplyEvent<RanSetup>
    {
        public void ApplyEvent(RanSetup @event)
        {
            Id = @event.AggregateId;
        }
    }
}