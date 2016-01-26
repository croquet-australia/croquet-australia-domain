using System;

namespace CroquetAustralia.CQRS
{
    public interface ICommand
    {
        Guid AggregateId { get; }
    }
}