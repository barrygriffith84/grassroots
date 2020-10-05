using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using grassroots.Models;

namespace grassroots.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<grassroots.Models.Activity> Activity { get; set; }
        public DbSet<grassroots.Models.Gathering> Gathering { get; set; }
        public DbSet<grassroots.Models.Location> Location { get; set; }
        public DbSet<grassroots.Models.GatheringUser> GatheringUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Restricts the cascade delete
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Gathering>()
                .HasMany(g => g.GatheringUsers)
                .WithOne(l => l.Gathering)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.GatheringUsers)
                .WithOne(l => l.User)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
            
