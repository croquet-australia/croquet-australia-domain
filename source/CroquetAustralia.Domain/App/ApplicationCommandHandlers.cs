using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CroquetAustralia.CQRS;
using CroquetAustralia.Domain.Users;

namespace CroquetAustralia.Domain.App
{
    internal class ApplicationCommandHandlers : IHandleCommand<RunSetup>
    {
        private readonly ICommandBus _commandBus;
        private readonly IEventStore _eventStore;

        public ApplicationCommandHandlers(ICommandBus commandBus, IEventStore eventStore)
        {
            _commandBus = commandBus;
            _eventStore = eventStore;
        }

        public async Task<IEnumerable<IAggregateEvents>> HandleCommandAsync(RunSetup command)
        {
            var applications = await _eventStore.GetAllAsync<Application>();

            if (applications.Any())
            {
                throw new SetupCannotBeRepeatedException();
            }

            await _commandBus.SendCommandAsync(new RegisterUser(Guid.NewGuid(), command.InitialAdministratorEmailAddress));

            return Events.For<Application>(command.AggregateId, new RanSetup(command));
        }
    }
}