using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Shared;

using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebApi.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
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

        public DbSet<TransCustomerDocTypeDef> TransCustomerDocTypeDefs { get; set; }
        public DbSet<TransCustomerDocSeriesDef> TransCustomerDocSeriesDefs { get; set; }
        public DbSet<TransCustomerDef> TransCustomerDefs { get; set; }
        public DbSet<MeasureUnit> MeasureUnits { get; set; }
        public DbSet<MaterialCategory> MaterialCategories { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<SupplierTransaction> SupplierTransactions { get; set; }
        public DbSet<WarehouseTransaction> WarehouseTransactions { get; set; }
        public DbSet<CustomerTransaction> CustomerTransactions { get; set; }
        public DbSet<BuyDocTypeDef> BuyDocTypeDefs { get; set; }
        public DbSet<BuyDocSeriesDef> BuyDocSeriesDefs { get; set; }
        public DbSet<BuyMaterialsDocLine> BuyMaterialsDocLines { get; set; }
        public DbSet<BuyMaterialsDocument> BuyMaterialsDocuments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region Part 1
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

            //----------------
            modelBuilder.Entity<TransSupplierDocSeriesDef>(entity =>
            {
                entity.HasIndex(c => c.Code)
                    .IsUnique();

                entity.HasOne(bd => bd.TransSupplierDocTypeDef)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TransSupplierDocTypeDef>(entity =>
            {
                entity.HasIndex(c => c.Code).IsUnique();

                entity.HasOne(bd => bd.TransSupplierDef)
                      .WithMany()
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion
            modelBuilder.Entity<TransSupplierDef>(entity =>
            {
                entity.HasOne(bd => bd.DebitTrans)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bd => bd.CreditTrans)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bd => bd.TurnOverTrans)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(c => c.Code)
                    .IsUnique();
            });

            //-----------------------
            //------------------------
            modelBuilder.Entity<TransCustomerDocSeriesDef>(entity =>
            {
                entity.HasIndex(c => c.Code)
                    .IsUnique();

                entity.HasOne(bd => bd.TransCustomerDocTypeDef)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TransCustomerDocTypeDef>(entity =>
            {
                entity.HasIndex(c => c.Code).IsUnique();

                entity.HasOne(bd => bd.TransCustomerDef)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<TransCustomerDef>(entity =>
            {
                entity.HasOne(bd => bd.DebitTrans)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bd => bd.CreditTrans)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bd => bd.TurnOverTrans)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(c => c.Code)
                    .IsUnique();
            });
            //--------------------

            modelBuilder.Entity<MeasureUnit>(entity =>
            {
                entity.HasIndex(p => p.Code).IsUnique();
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasIndex(c => c.Code).IsUnique();
                entity.HasOne(bd => bd.BuyMeasureUnit)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(bd => bd.SecondaryMeasureUnit)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(bd => bd.MainMeasureUnit)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(bd => bd.FpaDef)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(bd => bd.Company)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(bd => bd.MaterialCaterory)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<SupplierTransaction>(entity =>
            {
                entity.HasIndex(p => p.CreatorId);
                entity.HasIndex(p => p.TransDate);
                entity.HasOne(p => p.Company)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.FiscalPeriod)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Section)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.TransSupplierDocSeries)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.TransSupplierDocType)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Supplier)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.FpaDef)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<WarehouseTransaction>(entity =>
            {
                entity.HasIndex(p => p.CreatorId);
                entity.HasIndex(p => p.TransDate);
                entity.HasOne(p => p.Company)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.FiscalPeriod)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Section)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.TransWarehouseDocSeries)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.TransWarehouseDocType)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Material)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.FpaDef)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<CustomerTransaction>(entity =>
            {
                entity.HasIndex(p => p.CreatorId);
                entity.HasIndex(p => p.TransDate);
                entity.HasOne(p => p.Company)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.FiscalPeriod)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Section)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.TransCustomerDocSeries)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.TransCustomerDocType)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Customer)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.FpaDef)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<BuyDocTypeDef>(entity =>
            {
                entity.HasIndex(p => p.Code).IsUnique();
               
                entity.HasOne(p => p.TransSupplierDef)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.TransWarehouseDef)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Company)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<BuyDocSeriesDef>(entity =>
            {
                entity.HasIndex(p => p.Code).IsUnique();
               
                entity.HasOne(p => p.BuyDocTypeDef)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Company)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<BuyMaterialsDocLine>(entity =>
            {
                entity.HasOne(p => p.Material)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.BuyDocument)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BuyMaterialsDocument>(entity =>
            {
                entity.HasIndex(p => p.TransDate);
               

                entity.HasOne(p => p.Supplier)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Section)
               .WithMany()
               .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.FiscalPeriod)
               .WithMany()
               .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Company)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            });
        }
    }
}
