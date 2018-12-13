using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.Configuration.WarehouseTransDef
{
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var fMovements = _context.FinancialMovements.AsNoTracking().ToList();

            ViewData["AmtBuyTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["AmtExportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["AmtImportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["AmtInvoicedExportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["AmtInvoicedImportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["AmtSellTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["CompanyId"] = new SelectList(_context.Companies.AsNoTracking(), "Id", "Code");
            ViewData["VolBuyTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["VolExportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["VolImportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["VolInvoicedExportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["VolInvoicedImportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["VolSellTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            return Page();
        }

        [BindProperty]
        public TransWarehouseDef TransWarehouseDef { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TransWarehouseDefs.Add(TransWarehouseDef);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}