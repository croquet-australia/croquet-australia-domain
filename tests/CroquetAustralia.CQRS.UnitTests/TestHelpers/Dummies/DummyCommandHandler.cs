using System.Collections.Generic;
using System.Threading.Tasks;

namespace CroquetAustralia.CQRS.UnitTests.TestHelpers.Dummies
{
    public class DummyCommandHandler : IHandleCommand<DummyCommand1>
    {
        public static int HandleDummyCommand1;

        public Task<IEnumerable<IAggregateEvents>> HandleCommandAsync(DummyCommand1 command)
        {
            HandleDummyCommand1++;

            return Task.FromResult(Events.For<DummyAggregate>(command.AggregateId, new DummyEvent1(command.AggregateId)));
        }

        public static void Reset()
        {
            HandleDummyCommand1 = 0;
        }
    }
}