using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor
{
    public class AppSettingsModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public AppSettingsModel(ApiDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Name).AsNoTracking(), "Code", "Name");
        }
    }
}