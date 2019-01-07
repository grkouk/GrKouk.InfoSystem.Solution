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
            ViewData["TransWarehouseDefaultDocSeriesDefId"] =
                new SelectList(_context.TransWarehouseDocSeriesDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");


            List<SelectListItem> inventoryTransTypes = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value = WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumNoChange.ToString(),
                    Text = "No Change"
                },
                new SelectListItem()
                {
                    Value = WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumExport.ToString(),
                    Text = "Export"
                },
                new SelectListItem()
                {
                    Value = WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumImport.ToString(),
                    Text = "Import"
                },
                new SelectListItem()
                {
                    Value = WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumNegativeExport.ToString(),
                    Text = "Neg.Export"
                },
                new SelectListItem()
                {
                    Value = WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumNegativeImport.ToString(),
                    Text = "Neg.Import"
                }
            };
            List<SelectListItem> inventoryValueTransTypes = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value = WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumNoChange.ToString(),
                    Text = "No Change"
                },
                new SelectListItem()
                {
                    Value = WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumDecrease.ToString(),
                    Text = "Decrease"
                },
                new SelectListItem()
                {
                    Value = WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumIncrease.ToString(),
                    Text = "Increase"
                },
                new SelectListItem()
                {
                    Value = WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumNegativeDecrease.ToString(),
                    Text = "Neg.Decrease"
                },
                new SelectListItem()
                {
                    Value = WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumNegativeIncrease.ToString(),
                    Text = "Neg.Increase"
                }
            };
            ViewData["InventoryTransTypes"] = new SelectList(inventoryTransTypes, "Value", "Text");
            ViewData["InventoryValueTransTypes"] = new SelectList(inventoryValueTransTypes, "Value", "Text");
            ViewData["AmtBuyTransId"] = new SelectList(fMovements, "Id", "Name", 3);
           // ViewData["AmtExportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
           // ViewData["AmtImportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["AmtInvoicedExportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["AmtInvoicedImportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["AmtSellTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["CompanyId"] = new SelectList(_context.Companies.AsNoTracking(), "Id", "Code");
            ViewData["VolBuyTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            //ViewData["VolExportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            //ViewData["VolImportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["VolInvoicedExportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["VolInvoicedImportsTransId"] = new SelectList(fMovements, "Id", "Name", 3);
            ViewData["VolSellTransId"] = new SelectList(fMovements, "Id", "Name", 3);
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