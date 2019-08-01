using System;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Dtos.WebDtos.Diaries;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Configuration.BuyDocTypeDefinition
{
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IToastNotification _toastNotification;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context,IToastNotification toastNotification)
        {
            _context = context;
            this._toastNotification = toastNotification;
        }

        public IActionResult OnGet()
        {
            LoadCompbos();
            return Page();
        }

        private void LoadCompbos()
        {
          
            var usedPriceTypeList = Enum.GetValues(typeof(PriceTypeEnum))
                .Cast<PriceTypeEnum>()
                .Select(c => new UISelectTypeItem()
                {
                    Value = ((int)c).ToString(),
                    ValueInt = (int)c,
                    Text = c.GetDescription(),
                    Title = c.GetDescription()
                }).ToList();
            ViewData["UsedPrice"] = new SelectList(usedPriceTypeList, "Value", "Text");

            var warehouseItemNaturesList = Enum.GetValues(typeof(WarehouseItemNatureEnum))
                .Cast<WarehouseItemNatureEnum>()
                .Select(c => new UISelectTypeItem()
                {
                    ValueInt = (int)c,
                    Title = c.GetDescription()
                }).ToList();
            ViewData["warehouseItemNaturesList"] = new SelectList(warehouseItemNaturesList, "ValueInt", "Title"); 

           
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
          //  ViewData["TransSupplierDefId"] = new SelectList(_context.TransSupplierDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["TransTransactorDefId"] = new SelectList(_context.TransTransactorDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["TransWarehouseDefId"] = new SelectList(_context.TransWarehouseDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
        }

        [BindProperty]
        public InfoSystem.Domain.FinConfig.BuyDocTypeDef BuyDocTypeDef { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BuyDocTypeDefs.Add(BuyDocTypeDef);
            await _context.SaveChangesAsync();
            _toastNotification.AddSuccessToastMessage("Saved successfully");
            return RedirectToPage("./Index");
        }
    }
}