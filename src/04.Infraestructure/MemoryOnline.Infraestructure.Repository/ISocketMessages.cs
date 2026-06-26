
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Infraestructure.IRepository
{
    public interface ISocketMessages
    {
        Task SendMessageToUserAsync(Guid playerId, dynamic payload);

    }
}
