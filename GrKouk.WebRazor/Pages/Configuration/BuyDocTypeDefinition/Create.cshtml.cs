using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.FinConfig;
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
        private readonly IToastNotification toastNotification;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context,IToastNotification toastNotification)
        {
            _context = context;
            this.toastNotification = toastNotification;
        }

        public IActionResult OnGet()
        {
            LoadCompbos();
            return Page();
        }

        private void LoadCompbos()
        {
            List<SelectListItem> usedPriceTypeList = new List<SelectListItem>
            {

                new SelectListItem() {Value = PriceTypeEnum.PriceTypeEnumNetto.ToString(), Text = "Καθαρή Τιμή"},
                new SelectListItem() {Value = PriceTypeEnum.PriceTypeEnumBrutto.ToString(), Text = "Μικτή Τιμή"}

            };
            ViewData["UsedPrice"] = new SelectList(usedPriceTypeList, "Value", "Text");
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
            toastNotification.AddSuccessToastMessage("Saved successfully");
            return RedirectToPage("./Index");
        }
    }
}