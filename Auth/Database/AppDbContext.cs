using Auth.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Authentication> Auth { get; set; }

    }
}
