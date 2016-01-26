using CroquetAustralia.CQRS;

namespace CroquetAustralia.Domain.Users
{
    public class User : AggregateBase, IApplyEvent<RegisteredUser>
    {
        public string EmailAddress { get; private set; }

        public void ApplyEvent(RegisteredUser @event)
        {
            Id = @event.AggregateId;
            EmailAddress = @event.EmailAddress;
        }
    }
}