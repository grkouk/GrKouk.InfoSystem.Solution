using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.FinConfig;
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
            LoadCombos();
            return Page();
        }

        private void LoadCombos()
        {
            var fMovements = _context.FinancialMovements.AsNoTracking().ToList();
           
            List<SelectListItem> inventoryActions = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value = InventoryActionEnum.InventoryActionEnumNoChange.ToString(),
                    Text = "No Change"
                },
                new SelectListItem()
                {
                    Value = InventoryActionEnum.InventoryActionEnumExport.ToString(),
                    Text = "Export"
                },
                new SelectListItem()
                {
                    Value = InventoryActionEnum.InventoryActionEnumImport.ToString(),
                    Text = "Import"
                },
                new SelectListItem()
                {
                    Value = InventoryActionEnum.InventoryActionEnumNegativeExport.ToString(),
                    Text = "Neg.Export"
                },
                new SelectListItem()
                {
                    Value = InventoryActionEnum.InventoryActionEnumNegativeImport.ToString(),
                    Text = "Neg.Import"
                }
            };
            List<SelectListItem> inventoryValueActions = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value = InventoryValueActionEnum.InventoryValueActionEnumNoChange.ToString(),
                    Text = "No Change"
                },
                new SelectListItem()
                {
                    Value = InventoryValueActionEnum.InventoryValueActionEnumDecrease.ToString(),
                    Text = "Decrease"
                },
                new SelectListItem()
                {
                    Value = InventoryValueActionEnum.InventoryValueActionEnumIncrease.ToString(),
                    Text = "Increase"
                },
                new SelectListItem()
                {
                    Value = InventoryValueActionEnum.InventoryValueActionEnumNegativeDecrease.ToString(),
                    Text = "Neg.Decrease"
                },
                new SelectListItem()
                {
                    Value = InventoryValueActionEnum.InventoryValueActionEnumNegativeIncrease.ToString(),
                    Text = "Neg.Increase"
                }
            };
            ViewData["InventoryActions"] = new SelectList(inventoryActions, "Value", "Text");
            ViewData["InventoryValueActions"] = new SelectList(inventoryValueActions, "Value", "Text");
            ViewData["AmtBuyTransId"] = new SelectList(fMovements, "Id", "Name", 3);
          
            ViewData["AmtInvoicedExportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["AmtInvoicedImportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["AmtSellTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["CompanyId"] = new SelectList(_context.Companies.AsNoTracking(), "Id", "Code");
            ViewData["VolBuyTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["VolInvoicedExportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["VolInvoicedImportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["VolSellTransId"] = new SelectList(fMovements, "Id", "Name", 3);

            var dbSeriesList = _context.TransWarehouseDocSeriesDefs.OrderBy(p => p.Name).AsNoTracking();
            List<SelectListItem> seriesList = new List<SelectListItem>();
            seriesList.Add(new SelectListItem() { Value = 0.ToString(), Text = "{No Default series}" });
            foreach (var dbSeriesItem in dbSeriesList)
            {
                seriesList.Add(new SelectListItem() { Value = dbSeriesItem.Id.ToString(), Text = dbSeriesItem.Name });
            }
            ViewData["DefaultDocSeriesId"] = new SelectList(seriesList, "Value", "Text");
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