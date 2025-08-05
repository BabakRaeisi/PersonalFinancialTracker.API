using Microsoft.EntityFrameworkCore;
using PersonalFinancialTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinancialTracker.Infrastructure.Context
{
 
   public class AppDbContext : DbContext
   {
       public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
       {
       }

        // Your DbSet properties will go here
        // Example: public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Transaction> Transactions { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           base.OnModelCreating(modelBuilder);
           // Configure entity relationships here
       }
   }

    
}
