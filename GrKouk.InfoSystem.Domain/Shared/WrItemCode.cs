using System.ComponentModel.DataAnnotations;
using GrKouk.InfoSystem.Definitions;

namespace GrKouk.InfoSystem.Domain.Shared
{
    public class WrItemCode
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public WarehouseItemCodeTypeEnum CodeType { get; set; }
        [MaxLength(30)]
        public string Code { get; set; }
        public int TransactorId { get; set; }
        public int WarehouseItemId { get; set; }
        public WarehouseItem WarehouseItem { get; set; }
        public WarehouseItemCodeUsedUnitEnum CodeUsedUnit { get; set; }
        public double ToMainUnitRate { get; set; }

    }
}