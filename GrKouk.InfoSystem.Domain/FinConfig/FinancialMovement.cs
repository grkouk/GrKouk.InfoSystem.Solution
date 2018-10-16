using System.ComponentModel.DataAnnotations;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public class FinancialMovement
    {

        public int Id { get; set; }

        [MaxLength(15)]
        public string Code { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// + Αυξάνει
        /// - Μειώνει (αντιστροφή προσήμου κίνησης)
        /// ???????
        /// -+ Αυξάνει αρνητικά (αντιστροφή προσήμου κίνησης)
        /// -- Μειώνει αρνητικά (αντιστροφή προσήμου κίνησης)
        /// = Αμετάβλητο
        /// </summary>
        public string Action { get; set; }
    }
}