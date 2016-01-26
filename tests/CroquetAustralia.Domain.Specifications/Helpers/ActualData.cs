using System;
using CroquetAustralia.Domain.App;
using CroquetAustralia.Domain.Users;

namespace CroquetAustralia.Domain.Specifications.Helpers
{
    public class ActualData
    {
        public Application Application;
        public Exception Exception;
        public User User;

        public void TryCatch(Action action)
        {
            try
            {
                action();
                Exception = null;
            }
            catch (AggregateException exception)
            {
                Exception = exception.InnerExceptions.Count == 1 ? exception.InnerException : exception;
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
        }
    }
}