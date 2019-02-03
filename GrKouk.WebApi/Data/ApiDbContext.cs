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
        //#region Old Entities
        // public DbSet<TransSupplierDocTypeDef> TransSupplierDocTypeDefs { get; set; }
        // public DbSet<TransSupplierDocSeriesDef> TransSupplierDocSeriesDefs { get; set; }
        // public DbSet<TransSupplierDef> TransSupplierDefs { get; set; }

        //  public DbSet<TransCustomerDocTypeDef> TransCustomerDocTypeDefs { get; set; }
        // public DbSet<TransCustomerDocSeriesDef> TransCustomerDocSeriesDefs { get; set; }
        // public DbSet<TransCustomerDef> TransCustomerDefs { get; set; }
        // public DbSet<SupplierTransaction> SupplierTransactions { get; set; }
        // public DbSet<CustomerTransaction> CustomerTransactions { get; set; }
        //#endregion
        public DbSet<Company> Companies { get; set; }
        public DbSet<Transactor> Transactors { get; set; }
        public DbSet<FinDiaryTransaction> FinDiaryTransactions { get; set; }
        public DbSet<FinTransCategory> FinTransCategories { get; set; }
        public DbSet<TransactorType> TransactorTypes { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<FiscalPeriod> FiscalPeriods { get; set; }

        public DbSet<RevenueCentre> RevenueCentres { get; set; }
        public DbSet<CostCentre> CostCentres { get; set; }
        public DbSet<FinancialMovement> FinancialMovements { get; set; }

        public DbSet<FpaDef> FpaKategories { get; set; }
        public DbSet<TransWarehouseDef> TransWarehouseDefs { get; set; }
        public DbSet<TransWarehouseDocTypeDef> TransWarehouseDocTypeDefs { get; set; }
        public DbSet<TransWarehouseDocSeriesDef> TransWarehouseDocSeriesDefs { get; set; }
      
        public DbSet<MeasureUnit> MeasureUnits { get; set; }
        public DbSet<MaterialCategory> MaterialCategories { get; set; }
        public DbSet<Material> Materials { get; set; }
     
        public DbSet<WarehouseTransaction> WarehouseTransactions { get; set; }
      
        public DbSet<BuyDocTypeDef> BuyDocTypeDefs { get; set; }
        public DbSet<BuyDocSeriesDef> BuyDocSeriesDefs { get; set; }
        public DbSet<BuyDocLine> BuyDocLines { get; set; }
        public DbSet<BuyDocument> BuyDocuments { get; set; }
        public DbSet<TransExpenseDef> TransExpenseDefs { get; set; }
        public DbSet<TransactorTransaction> TransactorTransactions { get; set; }
        public DbSet<TransTransactorDef> TransTransactorDefs { get; set; }
        public DbSet<TransTransactorDocTypeDef> TransTransactorDocTypeDefs { get; set; }
        public DbSet<TransTransactorDocSeriesDef> TransTransactorDocSeriesDefs { get; set; }
        public DbSet<SellDocTypeDef> SellDocTypeDefs { get; set; }
        public DbSet<SellDocSeriesDef> SellDocSeriesDefs { get; set; }
        public DbSet<SellDocLine> SellDocLines { get; set; }
        public DbSet<SellDocument> SellDocuments { get; set; }
        public DbSet<MaterialCode> MaterialCodes { get; set; }
        public DbSet<DiaryDef> DiaryDefs { get; set; }

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
                   .HasOne(bd => bd.VolInvoicedExportsTrans)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransWarehouseDef>()
                   .HasOne(bd => bd.VolInvoicedImportsTrans)
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

           
            modelBuilder.Entity<Section>(entity =>
            {
                entity.HasIndex(c => c.Code)
                    .IsUnique();
            });
            modelBuilder.Entity<FiscalPeriod>(entity =>
            {
                entity.HasIndex(c => c.Code)
                    .IsUnique();
            });

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
               
            });
          
            modelBuilder.Entity<TransactorTransaction>(entity =>
            {
                entity.HasIndex(p => p.CreatorId);
                entity.HasIndex(p => p.TransDate);
                entity.HasOne(p => p.Company)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.TransTransactorDocType)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.TransTransactorDocSeries)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.FiscalPeriod)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Section)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
              
                entity.HasOne(p => p.Transactor)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            });
            modelBuilder.Entity<TransExpenseDef>(entity =>
            {
                entity.HasOne(bd => bd.Company)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasIndex(c => c.Code)
                    .IsUnique();
            });

            modelBuilder.Entity<BuyDocTypeDef>(entity =>
            {
                entity.HasIndex(p => p.Code).IsUnique();
               
               
                entity.HasOne(p => p.TransTransactorDef)
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
            modelBuilder.Entity<BuyDocLine>(entity =>
            {
                entity.HasOne(p => p.Material)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.BuyDocument)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BuyDocument>(entity =>
            {
                entity.HasIndex(p => p.TransDate);
                entity.HasOne(p => p.Transactor)
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
            modelBuilder.Entity<TransTransactorDef>(entity =>
            {
                entity.HasOne(bd => bd.Company)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(c => c.Code)
                    .IsUnique();
            });
            modelBuilder.Entity<TransTransactorDocTypeDef>(entity =>
            {
                entity.HasIndex(c => c.Code).IsUnique();

                entity.HasOne(bd => bd.TransTransactorDef)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<TransTransactorDocSeriesDef>(entity =>
            {
                entity.HasIndex(c => c.Code).IsUnique();

                entity.HasOne(bd => bd.TransTransactorDocTypeDef)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<SellDocTypeDef>(entity =>
            {
                entity.HasIndex(p => p.Code).IsUnique();

                entity.HasOne(p => p.TransTransactorDef)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.TransWarehouseDef)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Company)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<SellDocSeriesDef>(entity =>
            {
                entity.HasIndex(p => p.Code).IsUnique();

                entity.HasOne(p => p.SellDocTypeDef)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Company)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<SellDocLine>(entity =>
            {
                entity.HasOne(p => p.Material)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.SellDocument)
                    .WithMany(c=>c.SellDocLines)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SellDocument>(entity =>
            {
                entity.HasIndex(p => p.TransDate);

                entity.HasOne(p => p.Transactor)
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

            modelBuilder.Entity<MaterialCode>(entity =>
            {
                entity.HasKey(p => new
                {
                    p.CodeType, p.MaterialId, p.Code
                });
                entity.HasIndex(p => p.Code);

                entity.HasOne(p => p.Material)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
