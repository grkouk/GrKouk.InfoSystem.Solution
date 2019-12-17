﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Transactions.BuyMaterialsDoc
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public string SeekType { get; set; }
        public int RoutedCompanyId { get; set; }
        public int RoutedSectionId { get; set; }
        public bool InitialLoad = true;
        public int CopyFromId { get; set; }
        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> OnGetAsync(int? companyFilter,int? section, int? copyFromId)
        {
            RoutedCompanyId = (companyFilter ?? 0);
            RoutedSectionId = (section ?? 0);
            CopyFromId = (copyFromId ?? 0);

            if (CopyFromId > 0)
            {
                var buyMatDoc = await _context.BuyDocuments
                    .Include(b => b.Company)
                    .Include(b => b.FiscalPeriod)
                    .Include(b => b.BuyDocSeries)
                    .Include(b => b.BuyDocType)
                    .Include(b => b.Section)

                    .Include(b => b.Transactor)
                    .Include(b => b.BuyDocLines)
                    .ThenInclude(m => m.WarehouseItem)
                    .FirstOrDefaultAsync(m => m.Id == CopyFromId);
                if (buyMatDoc == null)
                {
                    return NotFound();
                }
                //ItemVm = _mapper.Map<BuyDocCreateAjaxDto>(buyMatDoc);
                CopyFromItemVm = _mapper.Map<BuyDocModifyDto>(buyMatDoc);
                ItemVm = new BuyDocCreateAjaxDto
                {
                    AmountDiscount = CopyFromItemVm.AmountDiscount,
                    AmountFpa = CopyFromItemVm.AmountFpa,
                    AmountNet = CopyFromItemVm.AmountNet,
                    BuyDocSeriesId = CopyFromItemVm.BuyDocSeriesId,
                    CompanyId = CopyFromItemVm.CompanyId,
                    Etiology = CopyFromItemVm.Etiology,
                    PaymentMethodId = CopyFromItemVm.PaymentMethodId,
                    TransactorId = CopyFromItemVm.TransactorId
                };
            }
            LoadCombos();
            return Page();
        }

        private void LoadCombos()
        {
            List<SelectListItem> seekTypes = new List<SelectListItem>
            {
                new SelectListItem() {Value = "NAME", Text = "Name"},
                new SelectListItem() {Value ="CODE", Text = "Code"},
                new SelectListItem() {Value = "BARCODE", Text = "Barcode"}
            };
            ViewData["SeekType"] = new SelectList(seekTypes, "Value", "Text");

            var supplierList = _context.Transactors.Where(s => s.TransactorType.Code == "SYS.SUPPLIER").OrderBy(s => s.Name).AsNoTracking();
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p=>p.Code).AsNoTracking(), "Id", "Code");
            ViewData["BuyDocSeriesId"] = new SelectList(_context.BuyDocSeriesDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["TransactorId"] = new SelectList(supplierList, "Id", "Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
        }

        [BindProperty]
        public BuyDocCreateAjaxDto ItemVm { get; set; }
        public BuyDocModifyDto CopyFromItemVm { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //var itemToAttach = _mapper.Map<BuyDocument>(ItemVm);
            //_context.BuyDocuments.Add(itemToAttach);
            //await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
       
    }
}