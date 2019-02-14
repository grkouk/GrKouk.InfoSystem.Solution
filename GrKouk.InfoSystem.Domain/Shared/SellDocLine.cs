﻿using System;
using System.ComponentModel.DataAnnotations;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class SellDocLine
    {
        public int Id { get; set; }
        public int SellDocumentId { get; set; }
        public virtual SellDocument SellDocument { get; set; }

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
        public decimal UnitPrice { get; set; }
        public decimal AmountFpa { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountDiscount { get; set; }
        [MaxLength(500)]
        public string Etiology { get; set; }
    }
}