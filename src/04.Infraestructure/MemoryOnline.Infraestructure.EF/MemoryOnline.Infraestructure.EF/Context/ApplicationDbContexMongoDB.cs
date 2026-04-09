using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.EF.Context
{
    public class ApplicationDbContextMongoDB : DbContext, IApplicationDbContext
    {
        public ApplicationDbContextMongoDB()
        {
        }

        public ApplicationDbContextMongoDB(DbContextOptions<ApplicationDbContextMongoDB> options) : base(options)
        {
        }

        public DbSet<GameState> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMongoDB("mongodb://admin:password123@localhost:27017", "gameDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GameState>(entity =>
            {
                entity.HasKey(g => g.Id);

                entity.OwnsMany(g => g.Players, p => { p.ToJson(); });
                entity.OwnsMany(g => g.Cards, c => { c.ToJson(); });
            });
        }

        async Task<int> IApplicationDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
