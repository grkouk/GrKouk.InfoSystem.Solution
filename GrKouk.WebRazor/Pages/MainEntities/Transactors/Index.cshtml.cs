using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.MainEntities.Transactors
{
    public class IndexModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;

        public IndexModel(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string NameSort { get; set; }
        public string NameSortIcon { get; set; }
        public string DateSort { get; set; }
        public string DateSortIcon { get; set; }
       
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public int PageSize { get; set; }
       public int CurrentTransactorTypeFilter { get; set; }
        //public int CurrentPageSize { get; set; }
        public PagedList<TransactorListDto> ListItems { get; set; }
        public async Task OnGetAsync(string sortOrder, string searchString,int? transactorTypeFilter, int? pageIndex, int? pageSize)
        {
            LoadFilters();

            PageSize = (int)((pageSize == null || pageSize == 0) ? 20 : pageSize);
           // CurrentPageSize = PageSize;
            CurrentSort = sortOrder;
            NameSort = sortOrder == "Name" ? "name_desc" : "Name";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }
            CurrentFilter = searchString;
            if (transactorTypeFilter==null)
            {
                transactorTypeFilter = 0;
            }
            CurrentTransactorTypeFilter = (int) (transactorTypeFilter ?? 0);

            IQueryable<Transactor> fullListIq = from s in _context.Transactors
                                                         select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                fullListIq = fullListIq.Where(s => s.Name.Contains(searchString));
            }

            if (transactorTypeFilter > 0)
            {
                fullListIq = fullListIq.Where(p => p.TransactorTypeId == transactorTypeFilter);
            }
            switch (sortOrder)
            {
                
                case "Name":
                    fullListIq = fullListIq.OrderBy(p => p.Name);
                    NameSortIcon = "fas fa-sort-alpha-up ";
                    DateSortIcon = "invisible";
                    break;
                case "name_desc":
                    fullListIq = fullListIq.OrderByDescending(p => p.Name);
                    NameSortIcon = "fas fa-sort-alpha-down ";
                    DateSortIcon = "invisible";
                    break;
                default:
                    fullListIq = fullListIq.OrderBy(p => p.Id);
                    break;
            }

            var t = fullListIq.ProjectTo<TransactorListDto>(_mapper.ConfigurationProvider);


            ListItems = await PagedList<TransactorListDto>.CreateAsync(
                t, pageIndex ?? 1, PageSize);

        }

        private void LoadFilters()
        {
            var dbTransactorTypes = _context.TransactorTypes.OrderBy(p => p.Code).AsNoTracking();
            List<SelectListItem> transactorTypes = new List<SelectListItem>();
            transactorTypes.Add(new SelectListItem() { Value = 0.ToString(), Text = "{All Types}" });
            foreach (var dbTransactorType in dbTransactorTypes)
            {
                transactorTypes.Add(new SelectListItem() { Value = dbTransactorType.Id.ToString(), Text = dbTransactorType.Code });
            }
            ViewData["TransactorTypeId"] = new SelectList(transactorTypes, "Value", "Text");
        }
    }
}
