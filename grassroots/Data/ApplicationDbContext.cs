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
        public DbSet<grassroots.Models.Event> Event { get; set; }
        public DbSet<grassroots.Models.Location> Location { get; set; }
    }
}
