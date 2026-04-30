using MemoryOnline.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.IRepository.Application
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
