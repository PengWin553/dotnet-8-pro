using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser> // Change inheritance from DbContext to IdentityDbContext<AppUser>
    {
        // 'ctor' + tab = creates a constructor
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; } // DbSet registers the Portfolios table
 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Step 4.1: Define composite primary key
            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId })); // Composite key of both FKs

            // Step 4.2: Configure AppUser -> Portfolios relationship
            builder.Entity<Portfolio>()
                .HasOne(u => u.AppUser) // Each Portfolio links to one AppUser
                .WithMany(u => u.Portfolios) // Each AppUser has many Portfolios
                .HasForeignKey(p => p.AppUserId); // Foreign key column
                // .OnDelete(DeleteBehavior.Cascade); // Optional: Delete Portfolios if User is deleted

            // Step 4.3: Configure Stock -> Portfolios relationship
            builder.Entity<Portfolio>()
                .HasOne(u => u.Stock) // Each Portfolio links to one Stock
                .WithMany(u => u.Portfolios) // Each Stock has many Portfolios
                .HasForeignKey(p => p.StockId); // Foreing key column
                // .OnDelete(DeleteBehavior.Cascade); // Optional: Delete Portfolios if Stock is deleted

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}