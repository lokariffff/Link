using Microsoft.EntityFrameworkCore;
using LinkShortener.Models;

namespace LinkShortener.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Link> Links { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Link>().HasIndex(l => l.ShortCode).IsUnique();
        }
    }
}