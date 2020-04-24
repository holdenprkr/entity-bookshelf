using System;
using System.Collections.Generic;
using System.Text;
using BookShelf.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookShelf.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }
        public DbSet<Genre> Genre { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Create a new user for Identity Framework
            ApplicationUser user = new ApplicationUser
            {
                FirstName = "Holden",
                UserName = "holden@admin.com",
                NormalizedUserName = "HOLDEN@ADMIN.COM",
                Email = "holden@admin.com",
                NormalizedEmail = "HOLDEN@ADMIN.COM",
                EmailConfirmed = false,
                LockoutEnabled = false,
                SecurityStamp = "7f434309-a4d9-48e9-9ebb-8803db794577",
                Id = "00000000-ffff-ffff-ffff-ffffffffffff"
            };
            var passwordHash = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHash.HashPassword(user, "Admin8*");
            modelBuilder.Entity<ApplicationUser>().HasData(user);

            // Create two cohorts
            modelBuilder.Entity<Genre>().HasData(
                new Genre()
                {
                    Id = 1,
                    Name = "Horror"
                },
                new Genre()
                {
                    Id = 2,
                    Name = "Sci-fi"
                },
                new Genre()
                {
                    Id = 3,
                    Name = "Fantasy"
                },
                new Genre()
                {
                    Id = 4,
                    Name = "Sci-fi"
                },
                new Genre()
                {
                    Id = 5,
                    Name = "Comedy"
                },
                new Genre()
                {
                    Id = 6,
                    Name = "Romance"
                }
            );
        }
    }
}
