using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Tekor.Data
{
    public class ApplicationDbContext : IdentityDbContext<CompanyAccount>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<CompanyAccount> Company { get; set; }
        public DbSet<Goal> Goal { get; set; }
        public DbSet<ActualGoalState> ActualGoalState { get; set; }
        public DbSet<Reward> Reward { get; set; }
        public DbSet<UserAccount> UserAcount { get; set; }

    }
}
