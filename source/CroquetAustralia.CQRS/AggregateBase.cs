using System;
using System.Collections.Generic;
using System.Linq;

namespace CroquetAustralia.CQRS
{
    public abstract class AggregateBase : IAggregate
    {
        private static readonly string ApplyEventMethodName = GetApplyEventMethodName();

        public Guid Id { get; protected set; }
        public IEnumerable<IEvent> Events { get; private set; }

        public void ApplyEvents(IEnumerable<IEvent> events)
        {
            ApplyEvents(events.ToArray());
        }

        private static string GetApplyEventMethodName()
        {
            return typeof(IApplyEvent<>).GetMethods().Single().Name;
        }

        private void ApplyEvents(IEvent[] events)
        {
            Events = events;

            var interfaces = (
                from i in GetType().GetInterfaces()
                where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IApplyEvent<>)
                select new {intefaceType = i, eventType = i.GetGenericArguments().Single()}).ToArray();

            foreach (var @event in events)
            {
                var @interface = interfaces.SingleOrDefault(i => i.eventType == @event.GetType());

                if (@interface == null)
                {
                    throw new ApplyEventInterfaceNotFoundException(this, @event);
                }

                var applyEventMethod = @interface.intefaceType.GetMethod(ApplyEventMethodName);

                applyEventMethod.Invoke(this, new object[] {@event});
            }
        }
    }
}