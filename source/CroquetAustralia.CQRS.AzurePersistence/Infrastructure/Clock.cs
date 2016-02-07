using System;

namespace CroquetAustralia.CQRS.AzurePersistence.Infrastructure
{
    public class Clock : IClock
    {
        private readonly Func<DateTime> _valueFactory;

        public Clock()
            : this(() => DateTime.UtcNow)
        {
        }

        public Clock(Func<DateTime> valueFactory)
        {
            _valueFactory = valueFactory;
        }

        public DateTime UtcNow => _valueFactory();
    }
}