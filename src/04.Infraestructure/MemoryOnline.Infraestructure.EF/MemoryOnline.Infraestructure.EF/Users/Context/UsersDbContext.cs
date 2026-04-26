using Hispalance.Infraestructure.DB.DBContext;
using MemoryOnline.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.EF.Users.Context
{
    public class UsersDbContext : DBContextInMemory
    {
        public UsersDbContext(IConfiguration config) : base(config)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>();

            base.OnModelCreating(modelBuilder);

            // Referentials
            //        modelBuilder.ApplyConfiguration(new UserResultConfiguration());
        }
    }
}
