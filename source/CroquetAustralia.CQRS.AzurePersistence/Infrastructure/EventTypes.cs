using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CroquetAustralia.CQRS.AzurePersistence.Infrastructure
{
    public class EventTypes : IEventTypes
    {
        private readonly ConstructorInfo[] _constructors;

        private EventTypes(IEnumerable<ConstructorInfo> constructors)
        {
            _constructors = constructors.ToArray();

            if (!_constructors.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(constructors), "Value cannot be empty.");
            }
        }

        public ConstructorInfo GetConstructorFor(string eventType)
        {
            var constructor = _constructors.SingleOrDefault(c => c.DeclaringType?.FullName == eventType);

            if (constructor == null)
            {
                throw new ArgumentOutOfRangeException(nameof(eventType), $"Cannot find constructor for event '{eventType}'.");
            }

            return constructor;
        }

        public static IEventTypes FromAssemblyContaining(Type type)
        {
            var eventType = typeof(IEvent);
            var constructors =
                from t in type.Assembly.GetTypes()
                where eventType.IsAssignableFrom(t)
                select t.GetConstructors().Single();

            return new EventTypes(constructors);
        }
    }
}