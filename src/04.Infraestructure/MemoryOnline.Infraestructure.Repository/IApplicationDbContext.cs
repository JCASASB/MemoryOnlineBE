using MemoryOnline.Domain.Entities.Game;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.IRepository
{
    public interface IApplicationDbContext
    {
        DbSet<GameState> Games { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
