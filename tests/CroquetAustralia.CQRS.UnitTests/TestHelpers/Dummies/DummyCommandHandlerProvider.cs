using System;

namespace CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies
{
    public class DummyCommandHandlerProvider : ICommandHandlerProvider
    {
        public object GetService(Type serviceType)
        {
            return serviceType == typeof(DummyCommandHandler) ? new DummyCommandHandler() : null;
        }
    }
}