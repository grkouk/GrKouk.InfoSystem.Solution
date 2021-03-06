﻿using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public class TransTransactorDef
    {
        public int Id { get; set; }
       
        [MaxLength(15)]
        public string Code { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [Display(Name = "Ενεργό")]
        public bool Active { get; set; }
        public FinActionsEnum FinancialTransAction { get; set; }
       
        [Display(Name = "Κίνηση Τζίρου")]
        public int TurnOverTransId { get; set; }
        public virtual FinancialMovement TurnOverTrans { get; set; }
        [Display(Name = "Default Series")]
        public int DefaultDocSeriesId { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}