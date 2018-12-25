using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class BuyMaterialsDocLine
    {
        public int Id { get; set; }

        public int BuyDocumentId { get; set; }
        public virtual BuyMaterialsDocument BuyDocument { get; set; }

        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }

        public Single FpaRate { get; set; }

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
        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public Single DiscountRate { get; set; }
        [MaxLength(500)]
        public string Etiology { get; set; }
    }
}
