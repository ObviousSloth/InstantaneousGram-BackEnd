using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InstantaneousGram_UserProfile.Models;

namespace InstantaneousGram_UserProfile.Data
{
    public class InstantaneousGram_UserProfileContext : DbContext
    {
        public InstantaneousGram_UserProfileContext (DbContextOptions<InstantaneousGram_UserProfileContext> options)
            : base(options)
        {
        }

        public DbSet<InstantaneousGram_UserProfile.Models.UserProfile> UserProfile { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
                .HasKey(u => u.Id); // Set primary key

            // Add any additional configuration here
        }
    }
}
