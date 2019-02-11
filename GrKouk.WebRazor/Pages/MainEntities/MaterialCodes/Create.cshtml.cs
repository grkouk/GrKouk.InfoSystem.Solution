using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.FinConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.MainEntities.MaterialCodes
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
            List<SelectListItem> codeTypes = new List<SelectListItem>
            {
                new SelectListItem() {Value = MaterialCodeTypeEnum.CodeTypeEnumCode.ToString(), Text = "Code"},
                new SelectListItem() {Value = MaterialCodeTypeEnum.CodeTypeEnumBarcode.ToString(), Text = "Barcode"},
                new SelectListItem() {Value = MaterialCodeTypeEnum.CodeTypeEnumSupplierCode.ToString(), Text = "Supplier Code"}
            };
            ViewData["CodeType"] = new SelectList(codeTypes, "Value", "Text");

            List<SelectListItem> codeUsedUnits = new List<SelectListItem>
            {
                new SelectListItem() {Value = MaterialCodeUsedUnitEnum.CodeUsedUnitEnumMain.ToString(), Text = "Main Unit"},
                new SelectListItem() {Value = MaterialCodeUsedUnitEnum.CodeUsedUnitEnumBuy.ToString(), Text = "Buy Unit"},
                new SelectListItem() {Value = MaterialCodeUsedUnitEnum.CodeUsedUnitEnumSecondary.ToString(), Text = "Secondary Unit"}
            };
            ViewData["CodeUsedUnit"] = new SelectList(codeUsedUnits, "Value", "Text");

            ViewData["MaterialId"] = new SelectList(_context.Materials.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
        }
        [BindProperty]
        public MaterialCode MaterialCode { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.MaterialCodes.Add(MaterialCode);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}