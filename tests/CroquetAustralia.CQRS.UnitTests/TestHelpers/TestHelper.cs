using System;

namespace CroquetAustralia.CQRS.UnitTests.TestHelpers
{
    public class TestHelper
    {
        public Exception CatchException(Action action)
        {
            try
            {
                action();
                return null;
            }
            catch (AggregateException exception)
            {
                return exception.InnerExceptions.Count == 1 ? exception.InnerExceptions[0] : exception;
            }
            catch (Exception exception)
            {
                return exception;
            }
        }
    }
}