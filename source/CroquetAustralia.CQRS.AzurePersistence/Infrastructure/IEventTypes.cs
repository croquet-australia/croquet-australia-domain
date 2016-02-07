using System.Reflection;

namespace CroquetAustralia.CQRS.AzurePersistence.Infrastructure
{
    public interface IEventTypes
    {
        ConstructorInfo GetConstructorFor(string eventType);
    }
}