using System;

namespace CroquetAustralia.CQRS.AzurePersistence.Infrastructure
{
    // Helps unit testing
    public interface IGuidFactory
    {
        Guid NewGuid();
    }
}