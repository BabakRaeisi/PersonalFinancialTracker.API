using Microsoft.EntityFrameworkCore;
using PersonalFinancialTracker.Core.Entities;
using PersonalFinancialTracker.Core.Enums;
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

        public DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure Transaction entity
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.TransactionId);
                
                entity.Property(t => t.UserId)
                    .IsRequired();
                
                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                    
                entity.Property(t => t.payor)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(t => t.payee)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(18, 2);
                    
                entity.Property(t => t.Description)
                    .HasMaxLength(500);
                    
                entity.Property(t => t.TypeOfTransaction)
                    .IsRequired()
                    .HasConversion<string>();
                    
                entity.Property(t => t.Created)
                    .IsRequired();
                    
                // Add indexes for better query performance
                entity.HasIndex(t => t.UserId)
                    .HasDatabaseName("IX_Transaction_UserId");
                    
                entity.HasIndex(t => t.Created)
                    .HasDatabaseName("IX_Transaction_Created");
                    
                entity.HasIndex(t => t.TypeOfTransaction)
                    .HasDatabaseName("IX_Transaction_TypeOfTransaction");
            });
        }
    }
}
