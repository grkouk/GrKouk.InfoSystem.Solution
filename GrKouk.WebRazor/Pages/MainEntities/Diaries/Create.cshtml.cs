using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.Diaries;
using GrKouk.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.MainEntities.Diaries
{
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            LoadCombos();
            return Page();
        }
        private void LoadCombos()
        {
            List<SelectListItem> diaryTypes = new List<SelectListItem>
            {
                new SelectListItem() {Value = DiaryTypeEnum.DiaryTypeEnumBuys.ToString(), Text = "Ημερολόγιο Αγορών"},
                new SelectListItem() {Value = DiaryTypeEnum.DiaryTypeEnumSales.ToString(), Text = "Ημερολόγιο Πωλήσεων"},
                new SelectListItem() {Value = DiaryTypeEnum.DiaryTypeEnumExpenses.ToString(), Text = "Ημερολόγιο Εξόδων"}
            };
            var BuyDocTypeListJs =  _context.BuyDocTypeDefs.OrderBy(p => p.Name)
                .Select(p => new
                {
                    label = p.Name,
                    title = p.Name,
                    value = p.Id
                }).ToArray();

            var SellDocTypeListJs = _context.SellDocTypeDefs.OrderBy(p => p.Name)
                .Select(p => new
                {
                    label = p.Name,
                    title = p.Name,
                    value = p.Id
                }).ToArray();

           ViewData["diaryTypes"] = new SelectList(diaryTypes, "Value", "Text");
           //ViewData["BuyDocTypeList"] = new SelectList(_context.BuyDocTypeDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
           ViewData["BuyDocTypeListJs"] = BuyDocTypeListJs;
           ViewData["SellDocTypeListJs"] = SellDocTypeListJs;
        }

        [BindProperty]
        public DiaryDto ItemVm { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var diaryDef = _mapper.Map<DiaryDef>(ItemVm);
            _context.DiaryDefs.Add(diaryDef);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}