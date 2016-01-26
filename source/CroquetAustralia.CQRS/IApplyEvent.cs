namespace CroquetAustralia.CQRS
{
    public interface IApplyEvent<in TEvent> where TEvent : IEvent
    {
        void ApplyEvent(TEvent @event);
    }
}