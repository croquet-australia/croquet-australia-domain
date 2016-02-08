using System;
using CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers;
using Ninject;

namespace CroquetAustralia.CQRS.AzurePersistence.Specifications.UnitTests
{
    public abstract class UnitTestBase
    {
        protected UnitTestBase()
            : this(TestContext.CreateKernel(), new Dummy())
        {
        }

        protected UnitTestBase(IKernel kernel, Dummy dummy)
        {
            Kernel = kernel;
            Dummy = dummy;
        }

        public IKernel Kernel { get; }
        public Dummy Dummy { get; }
    }
}