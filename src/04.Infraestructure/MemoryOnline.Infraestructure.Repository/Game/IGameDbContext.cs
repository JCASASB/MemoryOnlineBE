using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;

namespace MemoryOnline.Infraestructure.IRepository.Game
{
    public interface IGameDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
