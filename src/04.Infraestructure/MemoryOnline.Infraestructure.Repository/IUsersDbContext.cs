using MemoryOnline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.IRepository
{
    public interface IUsersDbContext
    {
        DbSet<Usuario> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
