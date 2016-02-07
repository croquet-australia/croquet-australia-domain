namespace CroquetAustralia.CQRS.AzurePersistence.Infrastructure
{
    public class EventSerializerRowKeyGenerator : IEventSerializerRowKeyGenerator
    {
        private readonly IClock _clock;
        private readonly IGuidFactory _guidFactory;

        public EventSerializerRowKeyGenerator(IClock clock, IGuidFactory guidFactory)
        {
            _clock = clock;
            _guidFactory = guidFactory;
        }

        public string GenerateRowKey()
        {
            return $"{_clock.UtcNow.Ticks:D19}-{_guidFactory.NewGuid()}";
        }
    }
}