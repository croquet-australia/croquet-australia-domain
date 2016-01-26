using System;

namespace CroquetAustralia.CQRS
{
    // ReSharper disable once InconsistentNaming
    internal static class IServiceProviderExtensions
    {
        internal static object GetRequiredService(this IServiceProvider serviceProvider, Type serviceType)
        {
            var service = serviceProvider.GetService(serviceType);

            if (service == null)
            {
                throw new InvalidOperationException($"Cannot find required service '{serviceType}'.");
            }

            return service;
        }
    }
}