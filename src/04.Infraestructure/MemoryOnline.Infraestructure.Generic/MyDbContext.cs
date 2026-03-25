using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.Generic;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.Repository
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options) , IMyDbContext
    {
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("loquesea");

            base.OnConfiguring(options);
        }
    }
}
