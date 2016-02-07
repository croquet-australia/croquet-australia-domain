namespace CroquetAustralia.CQRS.AzurePersistence.Infrastructure
{
    public interface IEventSerializerRowKeyGenerator
    {
        string GenerateRowKey();
    }
}