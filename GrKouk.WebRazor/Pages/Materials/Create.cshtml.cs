using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.Materials;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Materials
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

            List<SelectListItem> newList = new List<SelectListItem>
            {
                new SelectListItem() {Value = MaterialTypeEnum.MaterialTypeNormal.ToString(), Text = "Κανονικό"},
                new SelectListItem() {Value = MaterialTypeEnum.MaterialTypeSet.ToString(), Text = "Set"},
                new SelectListItem()
                {
                    Value = MaterialTypeEnum.MaterialTypeComposed.ToString(), Text = "Συντιθέμενο"
                }
            };

            ViewData["BuyMeasureUnitId"] = new SelectList(_context.MeasureUnits, "Id", "Code");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Code");
            ViewData["FpaDefId"] = new SelectList(_context.FpaKategories, "Id", "Code");
            ViewData["MainMeasureUnitId"] = new SelectList(_context.MeasureUnits, "Id", "Code");
            ViewData["MaterialCategoryId"] = new SelectList(_context.MaterialCategories, "Id", "Code");
            ViewData["SecondaryMeasureUnitId"] = new SelectList(_context.MeasureUnits, "Id", "Code");
            ViewData["MaterialType"] = new SelectList(newList,"Value","Text");
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