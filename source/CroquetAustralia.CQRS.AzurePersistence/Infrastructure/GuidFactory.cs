using System;

namespace CroquetAustralia.CQRS.AzurePersistence.Infrastructure
{
    public class GuidFactory : IGuidFactory
    {
        private readonly Func<Guid> _valueFactory;

        public GuidFactory()
            : this(Guid.NewGuid)
        {
        }

        public GuidFactory(Func<Guid> valueFactory)
        {
            _valueFactory = valueFactory;
        }

        public Guid NewGuid()
        {
            return _valueFactory();
        }
    }
}