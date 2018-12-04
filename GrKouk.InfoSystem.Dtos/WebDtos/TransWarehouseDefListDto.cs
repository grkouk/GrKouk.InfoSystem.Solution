using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GrKouk.InfoSystem.Dtos.WebDtos
{
    public class TransWarehouseDefListDto
    {
        public int Id { get; set; }

       
       
        public string Code { get; set; }
        public string Name { get; set; }
       
        public bool Active { get; set; }

        public string CompanyName { get; set; }

    }
}
