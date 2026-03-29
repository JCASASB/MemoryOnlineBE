using MemoryOnline.Domain.Entities;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOnline.Infraestructure.IRepository
{
    public interface IApplicationDbContext : IMyDbContext
    {
        DbSet<GameState> Games { get; set; }
        DbSet<Player> Players { get; set; }
        DbSet<Card> Cards { get; set; }
        DbSet<Usuario> Users { get; set; }

        // Método genérico para que el repositorio acceda a cualquier entidad
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        // Métodos para persistencia
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
