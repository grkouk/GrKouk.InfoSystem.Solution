﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.Materials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.MainEntities.Materials
{
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        public IActionResult OnGet()
        {
            LoadCombos();
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
            List<SelectListItem> materialNatures = new List<SelectListItem>
            {
               
                new SelectListItem() {Value = MaterialNatureEnum.MaterialNatureEnumMaterial.ToString(), Text = "Υλικό"},
                new SelectListItem() {Value = MaterialNatureEnum.MaterialNatureEnumService.ToString(), Text = "Υπηρεσία"},
                new SelectListItem() {Value = MaterialNatureEnum.MaterialNatureEnumExpense.ToString(), Text = "Δαπάνη"},
                new SelectListItem() {Value = MaterialNatureEnum.MaterialNatureEnumIncome.ToString(), Text = "Εσοδο"},
                new SelectListItem() {Value = MaterialNatureEnum.MaterialNatureEnumUndefined.ToString(), Text = "Undefined"},
                new SelectListItem() {Value = MaterialNatureEnum.MaterialNatureEnumFixedAsset.ToString(), Text = "Πάγιο"}
            };
            ViewData["BuyMeasureUnitId"] = new SelectList(_context.MeasureUnits.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["FpaDefId"] = new SelectList(_context.FpaKategories.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["MainMeasureUnitId"] = new SelectList(_context.MeasureUnits.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["MaterialCategoryId"] = new SelectList(_context.MaterialCategories.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["SecondaryMeasureUnitId"] = new SelectList(_context.MeasureUnits.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["MaterialNatures"] = new SelectList(materialNatures, "Value", "Text");
            ViewData["MaterialType"] = new SelectList(materialTypes, "Value", "Text");
        }

        [BindProperty]
        public MaterialCreateDto MaterialVm { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var materialToAttach = _mapper.Map<Material>(MaterialVm);
            _context.Materials.Add(materialToAttach);
            try
            {
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Material Created");
                return RedirectToPage("./Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toastNotification.AddErrorToastMessage(e.Message);
                return Page();
            }

          
        }
    }
}