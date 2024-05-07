using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InstantaneousGram_UserProfile.Models;

namespace InstantaneousGram_UserProfile.Data
{
    public class InstantaneousGram_UsersContextSQLite : DbContext
    {
       
        public InstantaneousGram_UsersContextSQLite(DbContextOptions<InstantaneousGram_UsersContextSQLite> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("InstantaneousGram_UserContext");
            }
        }

        public DbSet<InstantaneousGram_UserProfile.Models.User> Users { get; set; } = default!;


    }
}
