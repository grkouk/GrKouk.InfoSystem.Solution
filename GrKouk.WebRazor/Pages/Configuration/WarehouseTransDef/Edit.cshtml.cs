using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Configuration.WarehouseTransDef
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TransWarehouseDef TransWarehouseDef { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransWarehouseDef = await _context.TransWarehouseDefs
                .Include(t => t.AmtBuyTrans)
               // .Include(t => t.AmtExportsTrans)
               // .Include(t => t.AmtImportsTrans)
                .Include(t => t.AmtInvoicedExportsTrans)
                .Include(t => t.AmtInvoicedImportsTrans)
                .Include(t => t.AmtSellTrans)
                .Include(t => t.Company)
                .Include(t => t.VolBuyTrans)
                //.Include(t => t.VolExportsTrans)
               // .Include(t => t.VolImportsTrans)
                .Include(t => t.VolInvoicedExportsTrans)
                .Include(t => t.VolInvoicedImportsTrans)
                .Include(t => t.VolSellTrans).FirstOrDefaultAsync(m => m.Id == id);

            if (TransWarehouseDef == null)
            {
                return NotFound();
            }
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
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TransWarehouseDef).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransWarehouseDefExists(TransWarehouseDef.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TransWarehouseDefExists(int id)
        {
            return _context.TransWarehouseDefs.Any(e => e.Id == id);
        }
    }
}
