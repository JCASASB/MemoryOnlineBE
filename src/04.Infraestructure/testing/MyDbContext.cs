using MemoryOnline.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace testing
{
    public class RepositoryPatternDbContext(DbContextOptions<RepositoryPatternDbContext> options) : DbContext(options)
    {
        public virtual DbSet<Usuario> Usuario { get; set; }
    }
}
