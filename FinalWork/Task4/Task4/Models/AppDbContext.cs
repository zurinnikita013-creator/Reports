using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
//Журин Никита

namespace Task4.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=ProductDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
