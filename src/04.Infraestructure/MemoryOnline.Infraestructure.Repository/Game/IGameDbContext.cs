using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.IRepository.Game
{
    public interface IGameDbContext
    {
        DbSet<Match> Matches { get; set; }
          DbSet<Usuario> Usuarios { get; set; }
          DbSet<UserMatchResult> UsuarioResults { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
