#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using efcore.Models;

namespace efcore.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options)
            : base(options)
        {
        }

        public DbSet<efcore.Models.Product> Product { get; set; }
        public DbSet<efcore.Models.Category> Category { get; set; }
        public DbSet<efcore.Models.ProductCategory> ProductCategory { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ProductCategory>().HasKey(ES => new { ES.ProductId, ES.CategoryId });

            modelBuilder.Entity<Product>().HasData(
                new Product { id = 1, name = "chleb", price = 2 },
                new Product { id = 2, name = "maslo", price = 10 },
                new Product { id = 3, name = "ser", price = 15 },
                new Product { id = 4, name = "szynka", price = 20 },
                new Product { id = 5, name = "woda", price = 5 }
                );

            modelBuilder.Entity<Category>().HasData(
                new Category { ID = 1, name = "mieso", s_n = "m" },
                new Category { ID = 2, name = "pieczywo", s_n = "p" },
                new Category { ID = 3, name = "nabial", s_n = "n" },
                new Category { ID = 4, name = "jedzenie", s_n = "j" },
                new Category { ID = 5, name = "napoj", s_n = "np" }
                );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { id = 1, CategoryId = 1, ProductId = 4},
                new ProductCategory { id = 2, CategoryId = 2, ProductId = 1 },
                new ProductCategory { id = 3, CategoryId = 3, ProductId = 3 },
                new ProductCategory { id = 4, CategoryId = 3, ProductId = 2 },
                new ProductCategory { id = 5, CategoryId = 5, ProductId = 5 },
                new ProductCategory { id = 6, CategoryId = 4, ProductId = 1 },
                new ProductCategory { id = 7, CategoryId = 4, ProductId = 2 },
                new ProductCategory { id = 8, CategoryId = 4, ProductId = 3 },
                new ProductCategory { id = 9, CategoryId = 4, ProductId = 4 },
                new ProductCategory { id = 10, CategoryId = 4, ProductId = 5 }
                );
        }

    }
}
