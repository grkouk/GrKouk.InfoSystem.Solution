using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class BuyDocTransPaymentMapping
    {
        public int Id { get; set; }
        public int BuyDocumentId { get; set; }
        public BuyDocument BuyDocument { get; set; }

        public int TransactorTransactionId { get; set; }
        public TransactorTransaction TransactorTransaction { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal AmountUsed { get; set; }
    }
}
