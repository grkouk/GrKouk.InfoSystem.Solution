using System;
using System.Collections.Generic;
using System.Text;
using GrKouk.InfoSystem.Domain.Shared;

namespace GrKouk.InfoSystem.Domain.MediaEntities
{
    public class ProductMedia
    {
        public int Id { get; set; }
        public int MediaEntryId { get; set; }
        public MediaEntry MediaEntry { get; set; }
        public int ProductId { get; set; }
        public WarehouseItem Product { get; set; }
    }
}
