using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Definitions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.MainEntities.MaterialCodes
{
    [Authorize(Roles = "Admin")]
    public class CreateModelCopy : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        #region Fields
     
        public int ParentPageSize { get; set; }
        public int ParentPageIndex { get; set; }
        public string ParentSortOrder { get; set; }
        public string ParentSearchString { get; set; }
        public string ParentWarehouseItemNatureFilter { get; set; }
        public bool ParentFiltersVisible { get; set; }
        public bool ParentRowSelectorsVisible { get; set; }

        #endregion
        public CreateModelCopy(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string parentSortOrder, string parentSearchString,  bool parentFiltersVisible
            ,string parentWarehouseItemNatureFilter
            , bool parentRowSelectorsVisible , int? parentPageIndex, int? parentPageSize)
        {
            ParentFiltersVisible = parentFiltersVisible;
            ParentRowSelectorsVisible = parentRowSelectorsVisible;
            ParentSortOrder = parentSortOrder;
            ParentSearchString = parentSearchString;
            ParentWarehouseItemNatureFilter = parentWarehouseItemNatureFilter;
            if (parentPageIndex != null)
            {
                ParentPageIndex =(int) parentPageIndex;
            }

            if (parentPageSize != null)
            {
                ParentPageSize =(int) parentPageSize;
            }
            LoadCombos();
            return Page();
        }

        private void LoadCombos()
        {
            List<SelectListItem> codeTypes = new List<SelectListItem>
            {
                new SelectListItem() {Value = WarehouseItemCodeTypeEnum.CodeTypeEnumCode.ToString(), Text = "Code"},
                new SelectListItem() {Value = WarehouseItemCodeTypeEnum.CodeTypeEnumBarcode.ToString(), Text = "Barcode"},
                new SelectListItem() {Value = WarehouseItemCodeTypeEnum.CodeTypeEnumSupplierCode.ToString(), Text = "Supplier Code"}
            };
            ViewData["CodeType"] = new SelectList(codeTypes, "Value", "Text");

            List<SelectListItem> codeUsedUnits = new List<SelectListItem>
            {
                new SelectListItem() {Value = WarehouseItemCodeUsedUnitEnum.CodeUsedUnitEnumMain.ToString(), Text = "Main Unit"},
                new SelectListItem() {Value = WarehouseItemCodeUsedUnitEnum.CodeUsedUnitEnumBuy.ToString(), Text = "Buy Unit"},
                new SelectListItem() {Value = WarehouseItemCodeUsedUnitEnum.CodeUsedUnitEnumSecondary.ToString(), Text = "Secondary Unit"}
            };
            ViewData["CodeUsedUnit"] = new SelectList(codeUsedUnits, "Value", "Text");

            ViewData["WarehouseItemId"] = new SelectList(_context.WarehouseItems.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
        }
        [BindProperty]
        public WarehouseItemCode WarehouseItemCode { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.WarehouseItemsCodes.Add(WarehouseItemCode);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}