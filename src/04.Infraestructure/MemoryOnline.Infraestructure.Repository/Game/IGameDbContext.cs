using MemoryOnline.Domain.Entities.Game;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.IRepository.Game
{
    public interface IGameDbContext
    {
        DbSet<Match> Matches { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
