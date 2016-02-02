using System;

namespace CroquetAustralia.CQRS.UnitTests.TestHelpers
{
    public abstract class TestBase
    {
        protected readonly TestHelper TestHelper;

        protected TestBase() : this(new TestHelper())
        {
        }

        protected TestBase(TestHelper testHelper)
        {
            TestHelper = testHelper;
        }

        internal Exception CatchException(Action action)
        {
            return TestHelper.CatchException(action);
        }
    }
}