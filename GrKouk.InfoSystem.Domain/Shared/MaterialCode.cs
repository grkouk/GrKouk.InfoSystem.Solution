using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Definitions;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class MaterialCode   
    {
        public MaterialCodeTypeEnum CodeType { get; set; }
        [MaxLength(30)]
        public string Code { get; set; }
        public int TransactorId { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        public MaterialCodeUsedUnitEnum CodeUsedUnit { get; set; }

    }
}