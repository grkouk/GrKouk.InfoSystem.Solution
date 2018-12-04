using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Dtos.WebDtos;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.WarehouseTransDef
{
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;

        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IList<TransWarehouseDefListDto> ListItemsDtos { get; set; }

        public async Task OnGetAsync()
        {
            ListItemsDtos = await _context.TransWarehouseDefs
                .ProjectTo<TransWarehouseDefListDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
