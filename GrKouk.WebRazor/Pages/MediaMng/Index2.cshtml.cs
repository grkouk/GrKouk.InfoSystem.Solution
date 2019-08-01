using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.MediaEntities;
using GrKouk.InfoSystem.Dtos.WebDtos.Media;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.MediaMng
{
    public class IndexModel2 : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;

        public IndexModel2(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IList<MediaEntryDto> MediaEntry { get;set; }

        public async Task OnGetAsync()
        {
            var list = _mapper.Map<List<MediaEntryDto>>(await _context.MediaEntries.ToListAsync());
            foreach (var mediaItem in list)
            {
                
                mediaItem.Url = Url.Content("productimages/" + mediaItem.MediaFile);
            }
            MediaEntry = list;
        }
    }
}
