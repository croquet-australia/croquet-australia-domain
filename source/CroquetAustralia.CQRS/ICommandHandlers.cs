using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CroquetAustralia.CQRS
{
    public interface ICommandHandlers
    {
        Func<ICommand, Task<IEnumerable<IAggregateEvents>>> GetCommandHandler(ICommand command);
    }
}