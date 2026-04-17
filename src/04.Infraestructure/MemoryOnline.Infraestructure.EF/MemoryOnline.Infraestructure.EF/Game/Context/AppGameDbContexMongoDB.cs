using MemoryOnline.Infraestructure.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.EF.Game.Context
{
    public class AppGameDbContexMongoDB : AppGameDbContextBase, IApplicationDbContext
    {
        public AppGameDbContexMongoDB(DbContextOptions<AppGameDbContexMongoDB> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMongoDB("mongodb://admin:password123@localhost:27017", "gameDB");
            }
        }

        async Task<int> IApplicationDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
