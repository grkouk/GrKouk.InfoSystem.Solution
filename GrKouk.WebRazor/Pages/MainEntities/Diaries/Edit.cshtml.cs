using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.Diaries;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.MainEntities.Diaries
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public DiaryDef ItemVm { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ItemVm = await _context.DiaryDefs.FirstOrDefaultAsync(m => m.Id == id);

            if (ItemVm == null)
            {
                return NotFound();
            }
            LoadCombos();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ItemVm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiaryDefExists(ItemVm.Id))
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

        private bool DiaryDefExists(int id)
        {
            return _context.DiaryDefs.Any(e => e.Id == id);
        }
        private void LoadCombos()
        {
            List<SelectListItem> diaryTypes = new List<SelectListItem>
            {
                new SelectListItem() {Value = DiaryTypeEnum.DiaryTypeEnumBuys.ToString(), Text = "Ημερολόγιο Αγορών"},
                new SelectListItem() {Value = DiaryTypeEnum.DiaryTypeEnumSales.ToString(), Text = "Ημερολόγιο Πωλήσεων"},
                new SelectListItem() {Value = DiaryTypeEnum.DiaryTypeEnumExpenses.ToString(), Text = "Ημερολόγιο Εξόδων"}
            };
            var BuyDocTypeListJs = _context.BuyDocTypeDefs.OrderBy(p => p.Name)
                .Select(p => new DiaryDocTypeItem()
                {

                    Title = p.Name,
                    Value = p.Id
                }).ToList();


            var SellDocTypeListJs = _context.SellDocTypeDefs.OrderBy(p => p.Name)
                .Select(p => new DiaryDocTypeItem()
                {
                    Title = p.Name,
                    Value = p.Id
                }).ToList();

            ViewData["diaryTypes"] = new SelectList(diaryTypes, "Value", "Text");
            //ViewData["BuyDocTypeList"] = new SelectList(_context.BuyDocTypeDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["BuyDocTypeListJs"] = BuyDocTypeListJs;
            ViewData["SellDocTypeListJs"] = SellDocTypeListJs;
        }
    }
}
