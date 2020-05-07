﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.Transactors;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.MainEntities.Transactors
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        private int _id;

       
        public DetailsModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public TransactorDetailDto Item { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            LoadFilters();
            if (id == null)
            {
                return NotFound();
            }
            _id = (int)id;
            ViewData["TransactorId"] = id;
            var transactor = await _context.Transactors
                .Include(t=>t.TransactorCompanyMappings)
                .ThenInclude(x=>x.Company)
                .Include(t => t.TransactorType).FirstOrDefaultAsync(m => m.Id == id);

            if (transactor == null)
            {
                return NotFound();
            }
            Item=_mapper.Map<TransactorDetailDto>(transactor);
            var compList = transactor.TransactorCompanyMappings.Select(x => x.Company.Code).ToList();
            Item.Companies = String.Join(",", compList);
            var transactorTitle = $"{Item.TransactorTypeName} {Item.Name}";
            ViewData["ItemTitle"] = transactorTitle;
            ViewData["Title"] = $"{transactorTitle}-Details";
            return Page();
        }

        private void LoadFilters()
        {
            var currencyListJs = _context.Currencies.OrderBy(p => p.Name).AsNoTracking().ToList();
            ViewData["CurrencyListJs"] = currencyListJs;
            
            var companiesListJs = FiltersHelper.GetCompaniesFilterList(_context);
            ViewData["CompanyListJs"] = companiesListJs;

            var datePeriodListJs = DateFilter.GetDateFiltersSelectList();
            ViewData["DatePeriodListJs"] = datePeriodListJs;

        }
    }
}
