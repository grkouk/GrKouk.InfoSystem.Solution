using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Domain.RecurringTransactions
{
    public class RecurringTransDoc
    {
        private ICollection<RecurringTransDocLine> _docLines;
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; }

        public string TransRefCode { get; set; }
        [Required]
        public int SectionId { get; set; }
        //public virtual Section Section { get; set; }
        [Required]
        public int TransactorId { get; set; }
        //public virtual Transactor Transactor { get; set; }

        public int FiscalPeriodId { get; set; }
        //public virtual FiscalPeriod FiscalPeriod { get; set; }
        [Required]
        public int DocSeriesId { get; set; }
        [Required]
        public int DocTypeId { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal AmountFpa { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal AmountNet { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal AmountDiscount { get; set; }

        [MaxLength(500)]
        public string Etiology { get; set; }

        [Required]
        public int CompanyId { get; set; }
        //public virtual Company Company { get; set; }
        public int PaymentMethodId { get; set; }
        //public virtual PaymentMethod PaymentMethod { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
        public virtual ICollection<RecurringTransDocLine> DocLines
        {
            get => _docLines ?? (_docLines = new List<RecurringTransDocLine>());
            set => _docLines = value;
        }

    }
    public class RecurringTransDocLine
    {
        public int Id { get; set; }
        [ForeignKey("RecurringTransDoc")]
        public int RecurringTransDocId { get; set; }
        public virtual RecurringTransDoc RecurringTransDoc { get; set; }

        public int WarehouseItemId { get; set; }
        public virtual WarehouseItem WarehouseItem { get; set; }
        public int PrimaryUnitId { get; set; }
        //public virtual MeasureUnit PrimaryUnit { get; set; }
        public int SecondaryUnitId { get; set; }
        //public virtual MeasureUnit SecondaryUnit { get; set; }
        public Single Factor { get; set; }
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
        [Column(TypeName = "decimal(18, 4)")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal UnitExpenses { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal AmountFpa { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal AmountNet { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal AmountDiscount { get; set; }
        [MaxLength(500)]
        public string Etiology { get; set; }
    }
}
