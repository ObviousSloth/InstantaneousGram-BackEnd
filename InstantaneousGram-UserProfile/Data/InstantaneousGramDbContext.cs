using InstantaneousGram_UserProfile.Models;
using Microsoft.EntityFrameworkCore;

namespace InstantaneousGram_UserProfile.Data
{
    public class InstantaneousGramDbContext: DbContext
    {
        public InstantaneousGramDbContext(DbContextOptions<InstantaneousGramDbContext> options)
           : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Additional configurations can be added here
        }
    }
}
