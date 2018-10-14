using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.Shared;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebApi.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext( DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Transactor> Transactors { get; set; }
        public DbSet<FinDiaryTransaction> FinDiaryTransactions { get; set; }
        public DbSet<FinTransCategory> FinTransCategories { get; set; }
        
        public DbSet<RevenueCentre> RevenueCentres { get; set; }
        public DbSet<CostCentre> CostCentres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FinDiaryTransaction>().HasIndex(c => c.TransactionDate);

            modelBuilder.Entity<FinDiaryTransaction>()
                .HasOne(c => c.FinTransCategory)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FinTransCategory>().HasIndex(c => c.Code);

            modelBuilder.Entity<FinDiaryTransaction>()
                .HasOne(en => en.Transactor)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FinDiaryTransaction>()
                .HasOne(en => en.Company)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FinDiaryTransaction>()
                .HasOne(en => en.CostCentre)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FinDiaryTransaction>()
                .HasOne(en => en.RevenueCentre)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transactor>().HasIndex(c => c.Code);
            modelBuilder.Entity<Transactor>()
                .HasOne(en => en.TransactorType)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
