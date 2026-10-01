

namespace MemoryOnline.Infraestructure.IRepository
{
    public interface IHubDomainEvents
    {
        Task SendMessageToUserAsync(Guid playerId, dynamic payload);

    }
}
