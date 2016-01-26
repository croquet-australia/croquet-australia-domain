using System.Threading.Tasks;

namespace CroquetAustralia.CQRS
{
    public interface ICommandBus
    {
        Task SendCommandAsync(ICommand command);
    }
}