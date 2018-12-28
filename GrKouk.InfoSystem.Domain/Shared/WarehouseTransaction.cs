using GrKouk.InfoSystem.Domain.FinConfig;
using System;
using System.ComponentModel.DataAnnotations;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class WarehouseTransaction
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }

        public int TransWarehouseDocSeriesId { get; set; }
        public virtual TransWarehouseDocSeriesDef TransWarehouseDocSeries { get; set; }

        public int TransWarehouseDocTypeId { get; set; }
        public virtual TransWarehouseDocTypeDef TransWarehouseDocType { get; set; }
        /// <summary>
        /// Αριθμός Παραστατικού
        /// </summary>
        public string TransRefCode { get; set; }

        public int SectionId { get; set; }
        public virtual Section Section { get; set; }

        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }

        public int FiscalPeriodId { get; set; }
        public virtual FiscalPeriod FiscalPeriod { get; set; }

        /// <summary>
        /// Debit or Credit, for warehouse Import or Export
        /// </summary>
        public WarehouseTransactionTypeEnum TransactionType { get; set; }

        public WarehouseInventoryTransTypeEnum InventoryAction { get; set; }
        public WarehouseValueTransTypeEnum InventoryValueAction { get; set; }
        /// <summary>
        /// Ανάλογα με το sectionid το πεδίο αυτό περιέχει το id του αντικειμένου που ορίζει το sectionID 
        /// </summary>
        public int CreatorId { get; set; }
        public int PrimaryUnitId { get; set; }
        //public virtual MeasureUnit PrimaryUnit { get; set; }
        public int SecondaryUnitId { get; set; }
        //public virtual MeasureUnit SecondaryUnit { get; set; }

        /// <summary>
        /// Ποσότητα σε μονάδα μέτρησης 1
        /// </summary>
        public double Quontity1 { get; set; }
        /// <summary>
        /// Ποσότητα σε μονάδα μέτρησης 2
        /// </summary>
        public double Quontity2 { get; set; }
        public decimal FpaRate { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal UnitPrice { get; set; }  
        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }
        [MaxLength(500)]
        public string Etiology { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }

    }
}