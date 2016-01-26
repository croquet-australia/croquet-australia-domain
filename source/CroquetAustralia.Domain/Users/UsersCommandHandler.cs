using System.Collections.Generic;
using System.Threading.Tasks;
using CroquetAustralia.CQRS;

namespace CroquetAustralia.Domain.Users
{
    public class UsersCommandHandler : IHandleCommand<RegisterUser>
    {
        public Task<IEnumerable<IAggregateEvents>> HandleCommandAsync(RegisterUser command)
        {
            return Task.FromResult(Events.For<User>(command.AggregateId, new RegisteredUser(command)));
        }
    }
}