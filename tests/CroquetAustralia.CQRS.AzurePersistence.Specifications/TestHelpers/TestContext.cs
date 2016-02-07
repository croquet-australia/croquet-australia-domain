using System;
using CroquetAustralia.CQRS.AzurePersistence.Infrastructure;
using CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers.Dummies;
using Ninject;
using Ninject.Extensions.Conventions;

namespace CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers
{
    public class TestContext
    {
        private readonly IKernel _kernel;

        public TestContext(GivenData given, ActualData actual, TypeResolver typeResolver)
        {
            Given = given;
            Actual = actual;
            TypeResolver = typeResolver;

            _kernel = CreateKernel();
        }

        public ActualData Actual { get; private set; }
        public IEventDeserializer EventDeserializer => _kernel.Get<IEventDeserializer>();
        public IEventSerializer EventSerializer => _kernel.Get<IEventSerializer>();
        public IEventStore EventStore => _kernel.Get<IEventStore>();
        public GivenData Given { get; }
        public ITableNameResolver TableNameResolver => _kernel.Get<ITableNameResolver>();
        public TypeResolver TypeResolver { get; }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            kernel.Bind(syntax =>
            {
                syntax
                    .FromAssemblyContaining(typeof(ICommandBus), typeof(ITableNameResolver))
                    .SelectAllClasses()
                    .BindDefaultInterface();
            });

            kernel.Rebind<IEventStore>().ToConstructor(x => new AzureEventStore(GivenData.ConnectionString, x.Inject<TableNameResolver>(), x.Inject<IEventSerializer>(), x.Inject<IEventDeserializer>()));
            kernel.Rebind<ITableNameResolver>().ToConstructor(x => new TableNameResolver("zTest" + Guid.NewGuid().ToString().Replace("-", "") + "{0}")).InSingletonScope();
            kernel.Rebind<IEventTypes>().ToMethod(ctx => EventTypes.FromAssemblyContaining(typeof(DummyEventWithManyProperties))).InSingletonScope();

            return kernel;
        }
    }
}