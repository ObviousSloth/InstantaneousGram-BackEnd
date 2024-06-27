using Microsoft.EntityFrameworkCore;

using InstantaneousGram_ContentManagement.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Instantaneousgram_ContentManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ContentManagement> ContentManagements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configurations can be added here
        }
    }
}
