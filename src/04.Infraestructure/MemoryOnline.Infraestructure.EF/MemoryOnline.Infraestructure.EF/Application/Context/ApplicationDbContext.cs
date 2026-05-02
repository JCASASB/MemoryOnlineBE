using Hispalance.Infraestructure.DB.DBContext;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.EF.Application.Context
{
    public class ApplicationDbContext : DBContextInMemory
    {
        public ApplicationDbContext(IConfiguration config) : base(config)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>();

            modelBuilder.Entity<UserMatchResult>();

            base.OnModelCreating(modelBuilder);

            // Referentials
            //        modelBuilder.ApplyConfiguration(new UserResultConfiguration());
        }
    }
}
