using Microsoft.EntityFrameworkCore;

namespace Task2.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database=ProductDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}
