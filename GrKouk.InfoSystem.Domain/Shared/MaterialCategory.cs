using System.ComponentModel.DataAnnotations;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class MaterialCategory
    {
        public int Id { get; set; }

        [MaxLength(20)]
        [Required]
        public string Code { get; set; }

        [MaxLength(200)]
        [Required]
        public string Name { get; set; }
    }
}