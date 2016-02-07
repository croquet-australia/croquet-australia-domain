using System;
using CroquetAustralia.CQRS;
using CroquetAustralia.Domain.Specifications.Mocks;
using Ninject;

namespace CroquetAustralia.Domain.Specifications.Helpers
{
    public class TestContext
    {
        private readonly IKernel _kernel;

        public TestContext(GivenData given, ActualData actual)
        {
            Given = given;
            Actual = actual;

            _kernel = CreateKernel();
        }

        public ActualData Actual { get; private set; }
        public ICommandBus CommandBus => _kernel.Get<ICommandBus>();
        public IEventStore EventStore => _kernel.Get<IEventStore>();
        public GivenData Given { get; private set; }

        private IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            kernel.Bind<ICommandBus>().To<DomainCommandBus>().InSingletonScope();
            kernel.Bind<IEventStore>().To<MockEventStore>().InSingletonScope();
            kernel.Bind<ICommandHandlerProvider>().To<CommandHandlerProvider>();
            kernel.Bind<IServiceProvider>().ToConstant(kernel);

            return kernel;
        }
    }
}