using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.RecurringTransactions;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Transactions.RecurringTransactions
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public string SeekType { get; set; }
        public int RoutedCompanyId { get; set; }
        public int RoutedSectionId { get; set; }
        public bool InitialLoad = true;
        public int CopyFromId { get; set; }
        public int CreateFromId { get; set; }
        public RecurringDocTypeEnum CreateFromType { get; set; }

        public CreateModel(WebApi.Data.ApiDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> OnGetAsync(int? companyFilter, int? section, int? copyFromId, int? createFromId, int? createFromType )
        {
            RoutedCompanyId = companyFilter ?? 0;
            RoutedSectionId = section ?? 0;
            CopyFromId = copyFromId ?? 0;
            CreateFromId = createFromId ?? 0;
            if (createFromType>0)
            {
                CreateFromType = (RecurringDocTypeEnum) createFromType;
            }
            

            //if (CopyFromId > 0)
            //{
            //    var buyMatDoc = await _context.BuyDocuments
            //        .Include(b => b.Company)
            //        .Include(b => b.FiscalPeriod)
            //        .Include(b => b.BuyDocSeries)
            //        .Include(b => b.BuyDocType)
            //        .Include(b => b.Section)

            //        .Include(b => b.Transactor)
            //        .Include(b => b.BuyDocLines)
            //        .ThenInclude(m => m.WarehouseItem)
            //        .FirstOrDefaultAsync(m => m.Id == CopyFromId);
            //    if (buyMatDoc == null)
            //    {
            //        return NotFound();
            //    }
            //    //ItemVm = _mapper.Map<BuyDocCreateAjaxDto>(buyMatDoc);
            //    CopyFromItemVm = _mapper.Map<BuyDocModifyDto>(buyMatDoc);
            //    ItemVm = new BuyDocCreateAjaxDto
            //    {
            //        AmountDiscount = CopyFromItemVm.AmountDiscount,
            //        AmountFpa = CopyFromItemVm.AmountFpa,
            //        AmountNet = CopyFromItemVm.AmountNet,
            //        BuyDocSeriesId = CopyFromItemVm.BuyDocSeriesId,
            //        CompanyId = CopyFromItemVm.CompanyId,
            //        Etiology = CopyFromItemVm.Etiology,
            //        PaymentMethodId = CopyFromItemVm.PaymentMethodId,
            //        TransactorId = CopyFromItemVm.TransactorId
            //    };
            //}
            if (CreateFromId>0)
            {
                //RecurringTransDocCreateAjaxDto vm;
                switch (CreateFromType)
                {
                    case RecurringDocTypeEnum.BuyType:
                       
                            var buyMatDoc = await _context.BuyDocuments
                                .Include(b => b.Company)
                                .Include(b => b.FiscalPeriod)
                                .Include(b => b.BuyDocSeries)
                                .Include(b => b.BuyDocType)
                                .Include(b => b.Section)

                                .Include(b => b.Transactor)
                                .Include(b => b.BuyDocLines)
                                .ThenInclude(m => m.WarehouseItem)
                                .FirstOrDefaultAsync(m => m.Id == CreateFromId);
                            if (buyMatDoc == null)
                            {
                                return NotFound();
                            }
                            //ItemVm = _mapper.Map<BuyDocCreateAjaxDto>(buyMatDoc);
                            CopyFromItemVm = _mapper.Map<RecurringDocModifyDto>(buyMatDoc);
                            ItemVm = new RecurringTransDocCreateAjaxDto
                            {
                                NextTransDate=DateTime.Now.AddMonths(1),
                                RecurringFrequency="3M",
                                RecurringDocType=RecurringDocTypeEnum.BuyType,
                                AmountDiscount = CopyFromItemVm.AmountDiscount,
                                AmountFpa = CopyFromItemVm.AmountFpa,
                                AmountNet = CopyFromItemVm.AmountNet,
                                DocSeriesId = CopyFromItemVm.DocSeriesId,
                                CompanyId = CopyFromItemVm.CompanyId,
                                Etiology = CopyFromItemVm.Etiology,
                                PaymentMethodId = CopyFromItemVm.PaymentMethodId,
                                TransactorId = CopyFromItemVm.TransactorId
                            };
                            
                        
                        break;
                    case RecurringDocTypeEnum.SellType:
                        break;
                    default:
                        break;
                }
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
            var docTypes = Enum.GetValues(typeof(RecurringDocTypeEnum))
               .Cast<RecurringDocTypeEnum>()
               .Select(c => new SelectListItem()
               {
                   Value = c.ToString(),
                   Text = c.GetDescription()
               }).ToList();
            ViewData["DocType"] = new SelectList(docTypes, "Value", "Text");
            ViewData["RecurringFrequency"] = FiltersHelper.GetRecurringFrequencyList();
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p => p.Code).AsNoTracking(), "Id", "Code");
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            IQueryable transactorList;
            IQueryable docTypeList;
            if (CreateFromType==RecurringDocTypeEnum.BuyType)
            {
                transactorList = _context.Transactors.Where(s => s.TransactorType.Code == "SYS.SUPPLIER").OrderBy(s => s.Name).AsNoTracking();
                docTypeList = _context.BuyDocSeriesDefs.OrderBy(p => p.Name).AsNoTracking();
            }
            else
            {
                transactorList = _context.Transactors.Where(s => s.TransactorType.Code == "SYS.CUSTOMER" || s.TransactorType.Code == "SYS.DEPARTMENT").OrderBy(s => s.Name).AsNoTracking();
                docTypeList = _context.SellDocSeriesDefs.OrderBy(p => p.Name).AsNoTracking();
            }
           
            ViewData["TransactorId"] = new SelectList(transactorList, "Id", "Name");
            ViewData["DocSeriesId"] = new SelectList(docTypeList, "Id", "Name");
            
        }

        [BindProperty]
        public RecurringTransDocCreateAjaxDto ItemVm { get; set; }
        public RecurringDocModifyDto CopyFromItemVm { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }



            return RedirectToPage("./Index");
        }

    }
}