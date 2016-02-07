using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.WindowsAzure.Storage.Table;

namespace CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers
{
    public class ActualData
    {
        //public IDictionary<Guid, IEnumerable<IEvent>> Aggregates;
        public IAggregateEvents[] AggregateEventsCollection;
        public DynamicTableEntity DynamicTableEntity;
        public IEvent Event;
        public IEnumerable<IEvent> Events;
        public AzureEventStore EventStore;
        public Exception Exception;
        public object Result;
        public Stopwatch Stopwatch;

        public void TryCatch(Action action)
        {
            TryCatch(() =>
            {
                action();
                return 0;
            });
        }

        public T TryCatch<T>(Func<T> func)
        {
            Exception = null;

            try
            {
                return func();
            }
            catch (AggregateException exception)
            {
                Exception = exception.InnerExceptions.Count == 1 ? exception.InnerException : exception;
            }
            catch (Exception exception)
            {
                Exception = exception;
            }

            return default(T);
        }
    }
}