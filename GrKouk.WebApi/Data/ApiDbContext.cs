using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.FinConfig;
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
        public DbSet<TransactorType> TransactorTypes { get; set; }

        public DbSet<RevenueCentre> RevenueCentres { get; set; }
        public DbSet<CostCentre> CostCentres { get; set; }
        public DbSet<FinancialMovement> FinancialMovements { get; set; }

        public DbSet<FpaDef> FpaKategories { get; set; }
        public DbSet<TransWarehouseDef> TransWarehouseDefs { get; set; }
        public DbSet<TransWarehouseDocTypeDef> TransWarehouseDocTypeDefs { get; set; }
        public DbSet<TransWarehouseDocSeriesDef> TransWarehouseDocSeriesDefs { get; set; }
        public DbSet<TransSupplierDocTypeDef> TransSupplierDocTypeDefs { get; set; }
        public DbSet<TransSupplierDocSeriesDef> TransSupplierDocSeriesDefs { get; set; }
        public DbSet<TransSupplierDef> TransSupplierDefs { get; set; }



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

            modelBuilder.Entity<FpaDef>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<FinancialMovement>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<TransWarehouseDef>()
                   .HasOne(bd => bd.AmtExportsTrans)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDef>()
                   .HasOne(bd => bd.VolImportsTrans)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDef>()
                   .HasOne(bd => bd.VolExportsTrans)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDef>()
                   .HasOne(bd => bd.VolInvoicedExportsTrans)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDef>()
                   .HasOne(bd => bd.VolInvoicedImportsTrans)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDef>()
                   .HasOne(bd => bd.AmtImportsTrans)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDef>()
                   .HasOne(bd => bd.AmtInvoicedExportsTrans)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDef>()
                   .HasOne(bd => bd.AmtInvoicedImportsTrans)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDef>()
                   .HasOne(bd => bd.VolBuyTrans)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDef>()
                   .HasOne(bd => bd.AmtBuyTrans)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDef>()
                   .HasOne(bd => bd.VolSellTrans)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDef>()
                   .HasOne(bd => bd.AmtSellTrans)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDocTypeDef>()
                .HasOne(bd => bd.TransWarehouseDef)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDocSeriesDef>()
                .HasOne(bd => bd.TransWarehouseDocTypeDef)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDef>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<TransWarehouseDocTypeDef>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<TransWarehouseDocSeriesDef>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<TransSupplierDef>()
                .HasOne(bd => bd.DebitTrans)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransSupplierDef>()
                .HasOne(bd => bd.CreditTrans)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransSupplierDef>()
                .HasOne(bd => bd.TurnOverTrans)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransSupplierDocTypeDef>()
                .HasOne(bd => bd.TransSupplierDef)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransSupplierDocSeriesDef>()
                .HasOne(bd => bd.TransSupplierDocTypeDef)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransSupplierDef>()
                .HasIndex(c => c.Code)
                .IsUnique();
            modelBuilder.Entity<TransSupplierDocTypeDef>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<TransSupplierDocSeriesDef>()
                .HasIndex(c => c.Code)
                .IsUnique();

        }
    }
}
