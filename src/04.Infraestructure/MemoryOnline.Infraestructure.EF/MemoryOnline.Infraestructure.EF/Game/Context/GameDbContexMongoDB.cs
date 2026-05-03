using MemoryOnline.Infraestructure.IRepository.Game;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.EF.Game.Context
{
    public class GameDbContexMongoDB : GameDbContextBase, IGameDbContext
    {
        public GameDbContexMongoDB(DbContextOptions<GameDbContexMongoDB> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMongoDB("mongodb://admin:password123@localhost:27017", "memoryDB");
            }
        }

        async Task<int> IGameDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
