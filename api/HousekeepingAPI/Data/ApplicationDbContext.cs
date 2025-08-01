﻿using Microsoft.EntityFrameworkCore;
using HousekeepingAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace HousekeepingAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Models.Service> Services { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<ServiceSubCategory> ServiceSubCategories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ServiceSubCategory>()
                .HasKey(ssc => new { ssc.ServiceId, ssc.SubCategoryId });

            modelBuilder.Entity<ServiceSubCategory>()
                .HasOne(ssc => ssc.Service)
                .WithMany(s => s.ServiceSubCategory)
                .HasForeignKey(s => s.ServiceId);

            modelBuilder.Entity<ServiceSubCategory>()
                .HasOne(ssc => ssc.SubCategory)
                .WithMany(sc => sc.ServiceSubCategory)
                .HasForeignKey(s => s.SubCategoryId);

            List<IdentityRole> roles =
            [
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "Provider",
                    NormalizedName = "PROVIDER"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            ];
            
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
        
    }
}
