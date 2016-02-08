using System;

namespace CroquetAustralia.CQRS.AzurePersistence.Infrastructure
{
    public class EventSerializerException : Exception
    {
        public EventSerializerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}