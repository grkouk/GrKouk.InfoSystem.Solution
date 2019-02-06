﻿using System;
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
using GrKouk.WebRazor.Helpers;

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
            var diaryTypes = Enum.GetValues(typeof(DiaryTypeEnum))
                .Cast<DiaryTypeEnum>()
                .Select(c => new SelectListItem()
                {
                    Value = c.ToString(),
                    Text = c.GetDescription()
                }).ToList();

            #region MyRegion
            //foreach (DiaryTypeEnum value in Enum.GetValues(typeof(DiaryTypeEnum)))
            //{
            //    var a = value.GetDescription()
            //}
            //List<SelectListItem> diaryTypes = new List<SelectListItem>
            //{
            //    new SelectListItem() {Value = DiaryTypeEnum.DiaryTypeEnumBuys.ToString(), Text = "Ημερολόγιο Αγορών"},
            //    new SelectListItem() {Value = DiaryTypeEnum.DiaryTypeEnumSales.ToString(), Text = "Ημερολόγιο Πωλήσεων"},
            //    new SelectListItem() {Value = DiaryTypeEnum.DiaryTypeEnumExpenses.ToString(), Text = "Ημερολόγιο Εξόδων"}
            //};


            #endregion

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

            var TransactorDocTypeListJs = _context.TransTransactorDocTypeDefs.OrderBy(p => p.Name)
                .Select(p => new DiaryDocTypeItem()
                {
                    Title = p.Name,
                    Value = p.Id
                }).ToList();

            var WarehouseDocTypeListJs = _context.TransWarehouseDocTypeDefs.OrderBy(p => p.Name)
                .Select(p => new DiaryDocTypeItem()
                {
                    Title = p.Name,
                    Value = p.Id
                }).ToList();
            #region MyRegion
            //var BuyDocTypeListJs = _context.BuyDocTypeDefs.OrderBy(p => p.Name)
            //   .ProjectTo<DiaryDocTypeItem>(_mapper.ConfigurationProvider).ToList();

            //var SellDocTypeListJs = _context.SellDocTypeDefs.OrderBy(p => p.Name)
            //    .ProjectTo<DiaryDocTypeItem>(_mapper.ConfigurationProvider).ToList();


            #endregion

            var materialNatureList = Enum.GetValues(typeof(MaterialNatureEnum))
                .Cast<MaterialNatureEnum>()
                .Select(c => new UISelectTypeItem()
                {
                    Value = c.ToString(),
                    Title = c.GetDescription()
                }).ToList();

            var transactorTypeList = _context.TransactorTypes.OrderBy(p => p.Name)
                .Select(p => new UISelectTypeItem()
                {
                    Title = p.Name,
                    Value = p.Id.ToString()
                }).ToList();

            ViewData["diaryTypes"] = new SelectList(diaryTypes, "Value", "Text");
            ViewData["transactorTypes"] = new SelectList(transactorTypeList, "Value", "Title");
            ViewData["MaterialNatureTypes"] = new SelectList(materialNatureList, "Value", "Title");

            //ViewData["BuyDocTypeList"] = new SelectList(_context.BuyDocTypeDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["BuyDocTypeListJs"] = BuyDocTypeListJs;
            ViewData["SellDocTypeListJs"] = SellDocTypeListJs;
            ViewData["TransactorDocTypeListJs"] = TransactorDocTypeListJs;
            ViewData["WarehouseDocTypeListJs"] = WarehouseDocTypeListJs;

            ViewData["MaterialNaturesList"] = materialNatureList;
            ViewData["TransactorTypeList"] = transactorTypeList;
        }
    }
}