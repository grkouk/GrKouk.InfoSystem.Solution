using System.ComponentModel.DataAnnotations;

namespace GrKouk.InfoSystem.Domain.FinConfig
{
    public class SalesChannel
    {
        public int Id { get; set; }
        [MaxLength(15)]
        public string Code { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }

    }
}