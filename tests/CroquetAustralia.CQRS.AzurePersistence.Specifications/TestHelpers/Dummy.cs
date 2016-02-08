using System;
using System.Linq;

namespace CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers
{
    public class Dummy : OpenMagic.Dummy
    {
        public IEvent Event(Type eventType)
        {
            try
            {
                var constructorInfo = eventType.GetConstructors().Single();
                var parameterInfos = constructorInfo.GetParameters();
                var parameterValues = parameterInfos.Select(p => Value(p.ParameterType));
                var obj = constructorInfo.Invoke(parameterValues.ToArray());

                return (IEvent) obj;

            }
            catch (Exception exception)
            {
                throw new Exception($"Cannot create dummy event '{eventType}'.", exception);
            }
        }
    }
}