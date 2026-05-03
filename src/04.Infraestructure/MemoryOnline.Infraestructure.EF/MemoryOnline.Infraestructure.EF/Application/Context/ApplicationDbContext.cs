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
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);

                // Definir la relación 1 a muchos
                entity.HasMany(u => u.Results)         // Un Usuario tiene muchos Results
                      .WithOne()                       // Cada Result pertenece a un Usuario
                      .HasForeignKey("UsuarioId")      // EF creará esta Shadow Property en la DB
                      .OnDelete(DeleteBehavior.Cascade); // Si borras al usuario, se borran sus resultados
            });

            modelBuilder.Entity<UserMatchResult>(entity =>
            {
                entity.HasKey(r => r.Id); // Asumiendo que UserMatchResult tiene un Id
                entity.Property(r => r.UsuarioId).IsRequired();
            });

            base.OnModelCreating(modelBuilder);

            // Referentials
            //        modelBuilder.ApplyConfiguration(new UserResultConfiguration());
        }
    }
}
