﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseItems;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.MainEntities.Materials
{
    public class EditModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public EditModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public WarehouseItemModifyDto WarehouseItemVm { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           var  materialToModify = await _context.WarehouseItems
                .Include(m => m.BuyMeasureUnit)
                .Include(m => m.Company)
                .Include(m => m.FpaDef)
                .Include(m => m.MainMeasureUnit)
                .Include(m => m.MaterialCaterory)
                .Include(m => m.SecondaryMeasureUnit).FirstOrDefaultAsync(m => m.Id == id);

            if (materialToModify == null)
            {
                return NotFound();
            }

            WarehouseItemVm = _mapper.Map<WarehouseItemModifyDto>(materialToModify);
           LoadCombos();
          // _toastNotification.AddInfoToastMessage("Welcome to edit page");
            return Page();
        }
        private void LoadCombos()
        {

            List<SelectListItem> materialTypes = new List<SelectListItem>
            {
                new SelectListItem() {Value = MaterialTypeEnum.MaterialTypeNormal.ToString(), Text = "Κανονικό"},
                new SelectListItem() {Value = MaterialTypeEnum.MaterialTypeSet.ToString(), Text = "Set"},
                new SelectListItem() {Value = MaterialTypeEnum.MaterialTypeComposed.ToString(), Text = "Συντιθέμενο"}
            };
            var materialNatures = Enum.GetValues(typeof(WarehouseItemNatureEnum))
                .Cast<WarehouseItemNatureEnum>()
                .Select(c => new SelectListItem()
                {
                    Value = c.ToString(),
                    Text = c.GetDescription()
                }).ToList();

            //List<SelectListItem> materialNatures = new List<SelectListItem>
            //{
            //    new SelectListItem() {Value = WarehouseItemNatureEnum.WarehouseItemNatureMaterial.ToString(), Text = "Υλικό"},
            //    new SelectListItem() {Value = WarehouseItemNatureEnum.WarehouseItemNatureService.ToString(), Text = "Υπηρεσία"},
            //    new SelectListItem() {Value = WarehouseItemNatureEnum.WarehouseItemNatureExpense.ToString(), Text = "Δαπάνη"},
            //    new SelectListItem() {Value = WarehouseItemNatureEnum.WarehouseItemNatureIncome.ToString(), Text = "Εσοδο"},
            //    new SelectListItem() {Value = WarehouseItemNatureEnum.WarehouseItemNatureUndefined.ToString(), Text = "Undefined"},
            //    new SelectListItem() {Value = WarehouseItemNatureEnum.WarehouseItemNatureFixedAsset.ToString(), Text = "Πάγιο"}
            //};
            ViewData["BuyMeasureUnitId"] = new SelectList(_context.MeasureUnits.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["FpaDefId"] = new SelectList(_context.FpaKategories.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["MainMeasureUnitId"] = new SelectList(_context.MeasureUnits.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["MaterialCategoryId"] = new SelectList(_context.MaterialCategories.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["SecondaryMeasureUnitId"] = new SelectList(_context.MeasureUnits.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["MaterialNatures"] = new SelectList(materialNatures, "Value", "Text");
            ViewData["MaterialType"] = new SelectList(materialTypes, "Value", "Text");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _toastNotification.AddAlertToastMessage("Please see errors");
                return Page();
            }

            var materialToAttach = _mapper.Map<WarehouseItem>(WarehouseItemVm);

            _context.Attach(materialToAttach).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("WarehouseItem changes saved");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(WarehouseItemVm.Id))
                {
                    _toastNotification.AddErrorToastMessage("WarehouseItem was not found");
                    return NotFound();
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Concurency error");
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MaterialExists(int id)
        {
            return _context.WarehouseItems.Any(e => e.Id == id);
        }
    }
}
