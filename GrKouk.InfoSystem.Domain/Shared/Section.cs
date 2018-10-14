using System.ComponentModel.DataAnnotations;

namespace GrKouk.InfoSystem.Domain.Shared
{
    /// <summary>
    /// Κυκλώματα
    /// </summary>
    public class Section
    {
        public int Id { get; set; }

        [MaxLength(15)]
        public string Code { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string SystemName { get; set; }
    }
}