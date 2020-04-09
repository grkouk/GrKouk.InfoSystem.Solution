using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Domain.RecurringTransactions
{
    public class RecurringTransDoc
    {
        private ICollection<RecurringTransDocLine> _docLines;
        public int Id { get; set; }
        [MaxLength(2)]
        public string RecurringFrequency { get; set; }

        public RecurringDocType RecurringDocType { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime NextTransDate { get; set; }

        public string TransRefCode { get; set; }
        [Required]
        public int SectionId { get; set; }
        //public virtual Section Section { get; set; }
        [Required]
        public int TransactorId { get; set; }
        public virtual Transactor Transactor { get; set; }

        //public int FiscalPeriodId { get; set; }
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
        public virtual Company Company { get; set; }
        public int PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
        public virtual ICollection<RecurringTransDocLine> DocLines
        {
            get => _docLines ?? (_docLines = new List<RecurringTransDocLine>());
            set => _docLines = value;
        }

    }
}
