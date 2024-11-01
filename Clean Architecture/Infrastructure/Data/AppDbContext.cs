using Clean_Architecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clean_Architecture.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Project> Projects { get; set; }

    }
}
