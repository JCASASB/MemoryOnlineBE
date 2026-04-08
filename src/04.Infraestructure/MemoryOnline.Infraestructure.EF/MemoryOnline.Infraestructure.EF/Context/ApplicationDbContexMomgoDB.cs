using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

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
                entity.ToCollection("games");

                // Players and Cards are embedded documents
                entity.OwnsMany(g => g.Players, p =>
                {
                    p.Property(x => x.Id).HasConversion<string>();
                });

                entity.OwnsMany(g => g.Cards, c =>
                {
                    c.Property(x => x.Id).HasConversion<string>();
                });
            });
        }

        async Task<int> IApplicationDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
