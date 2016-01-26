using System.Collections.Generic;
using System.Threading.Tasks;

namespace CroquetAustralia.CQRS
{
    public interface IHandleCommand<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IAggregateEvents>> HandleCommandAsync(TCommand command);
    }
}