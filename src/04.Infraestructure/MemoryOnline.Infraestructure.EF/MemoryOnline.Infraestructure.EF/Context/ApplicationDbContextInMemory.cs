using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.EF.Context
{
    public class ApplicationDbContextInMemory : DbContext, IApplicationDbContext
    {
        public ApplicationDbContextInMemory()
        {
        }

        public ApplicationDbContextInMemory(DbContextOptions<ApplicationDbContextInMemory> options) : base(options)
        {
        }

        public DbSet<GameState> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("TuCadenaDeConexion");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GameState>(entity =>
            {
                entity.HasKey(g => g.Id);

                // Use OwnsMany for embedded documents (same as MongoDB)
                entity.OwnsMany(g => g.Players, p =>
                {
                    p.WithOwner().HasForeignKey("GameStateId");
                    p.HasKey(x => x.Id);
                });

                entity.OwnsMany(g => g.Cards, c =>
                {
                    c.WithOwner().HasForeignKey("GameStateId");
                    c.HasKey(x => x.Id);
                });
            });
        }

        async Task<int> IApplicationDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
