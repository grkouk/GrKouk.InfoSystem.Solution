using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseTransactions;
using GrKouk.WebRazor.Helpers;

namespace GrKouk.WebRazor.Pages.Transactions.WarehouseTransMng
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
        public int CurrentPageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public PagedList<WarehouseTransListDto> ListItems { get; set; }
        public async Task OnGetAsync(string sortOrder, string searchString, int? pageIndex, int? pageSize)
        {
            PageSize = (int)((pageSize == null || pageSize == 0) ? 20 : pageSize);
            CurrentPageSize = PageSize;
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
            IQueryable<WarehouseTransaction> fullListIq = from s in _context.WarehouseTransactions
                select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                fullListIq = fullListIq.Where(s => s.Material.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Date":
                    fullListIq = fullListIq.OrderBy(p => p.TransDate);
                    DateSortIcon = "fas fa-sort-numeric-up ";
                    NameSortIcon = "invisible";
                    break;
                case "date_desc":
                    fullListIq = fullListIq.OrderByDescending(p => p.TransDate);
                    DateSortIcon = "fas fa-sort-numeric-down ";
                    NameSortIcon = "invisible";
                    break;
                case "Name":
                    fullListIq = fullListIq.OrderBy(p => p.Material.Name);
                    NameSortIcon = "fas fa-sort-alpha-up ";
                    DateSortIcon = "invisible";
                    break;
                case "name_desc":
                    fullListIq = fullListIq.OrderByDescending(p => p.Material.Name);
                    NameSortIcon = "fas fa-sort-alpha-down ";
                    DateSortIcon = "invisible";
                    break;
                default:
                    fullListIq = fullListIq.OrderBy(p => p.Id);
                    break;
            }
            var t = fullListIq.ProjectTo<WarehouseTransListDto>(_mapper.ConfigurationProvider);
            ListItems = await PagedList<WarehouseTransListDto>.CreateAsync(
                t, pageIndex ?? 1, PageSize);
        }
    }
}
