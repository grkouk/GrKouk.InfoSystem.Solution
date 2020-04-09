﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.Diaries;
using GrKouk.InfoSystem.Dtos.WebDtos.Transactors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.MainEntities.Transactors
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly IMapper _mapper;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context, IToastNotification toastNotification, IMapper mapper)
        {
            _context = context;
            _toastNotification = toastNotification;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            LoadCombos();
            return Page();
        }
        private void LoadCombos()
        {
            var companiesListJs = _context.Companies.OrderBy(p => p.Name)
                .Select(p => new DiaryDocTypeItem()
                {
                    Title = p.Name,
                    Value = p.Id
                }).ToList();
            ViewData["TransactorTypeId"] = new SelectList(_context.TransactorTypes.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            //ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["CompaniesListJs"] = companiesListJs;
        }
        [BindProperty]
        public TransactorCreateDto ItemVm { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            
            var transactorToAdd = _mapper.Map<Transactor>(ItemVm);
            
            _context.Transactors.Add(transactorToAdd);

            if (!String.IsNullOrEmpty(ItemVm.SelectedCompanies))
            {
                var listOfCompanies = ItemVm.SelectedCompanies.Split(",");
                //bool fl = true;
                foreach (var listOfCompany in listOfCompanies)
                {
                    int companyId;
                    int.TryParse(listOfCompany, out companyId);
                    if (companyId>0)
                    {
                        transactorToAdd.TransactorCompanyMappings.Add(new TransactorCompanyMapping
                        {
                            CompanyId = companyId,
                            TransactorId = transactorToAdd.Id
                        });
                        //TODO: remove when companyid column removed for transactor entity
                        //if (fl)
                        //{
                        //    transactorToAdd.CompanyId = companyId;
                        //    fl = false;
                        //}
                        
                    }
                }
               
            }
            
            try
            {
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Transactor saved!");
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage(e.Message);
                Console.WriteLine(e);
            }

            return RedirectToPage("./Index");
        }
    }
}