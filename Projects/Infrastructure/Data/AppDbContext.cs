using Projects.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Projects.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Project> Projects { get; set; }

    }
}
