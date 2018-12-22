using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyMaterialsDocs;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace GrKouk.WebRazor.Pages.Transactions.BuyMaterialsDoc
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
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(p=>p.Code).AsNoTracking(), "Id", "Code");
            ViewData["MaterialDocSeriesId"] = new SelectList(_context.BuyMaterialDocSeriesDefs.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
            ViewData["SupplierId"] = new SelectList(_context.Transactors.OrderBy(p => p.Name).AsNoTracking(), "Id", "Name");
        }

        [BindProperty]
        public BuyMaterialsDocCreateDto ItemVm { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var itemToAttach = _mapper.Map<BuyMaterialsDocument>(ItemVm);
            _context.BuyMaterialsDocuments.Add(itemToAttach);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        public async Task<IActionResult> OnPostAjaxPostAsync([FromBody] BuyMaterialsDocCreateAjaxDto data)
        {
            Debug.Print(data.TransDate.ToString());
            foreach (var dataBuyDocLine in data.BuyDocLines)
            {
                Debug.WriteLine("Lines ");
                Debug.WriteLine(dataBuyDocLine.MaterialId.ToString());
                Debug.WriteLine(dataBuyDocLine.Amount.ToString());
                Debug.WriteLine(dataBuyDocLine.Q1.ToString());
                Debug.WriteLine(dataBuyDocLine.Price.ToString());


            }
            
            return new OkResult(); 
        }
    }
}