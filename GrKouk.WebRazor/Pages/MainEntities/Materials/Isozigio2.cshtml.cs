using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseTransactions;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Pages.MainEntities.Materials
{
    [Authorize(Roles = "Admin")]
    public class Isozigio2Model : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IMapper _mapper;

        public Isozigio2Model(GrKouk.WebApi.Data.ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public decimal sumImportsVolume = 0;
        public decimal sumExportsVolume = 0;
        public decimal sumImportsValue = 0;
        public decimal sumExportsValue = 0;

       
        public int PageSizeKartela { get; set; }
        public string TransactorName { get; set; }
        public int TransactorId { get; set; }

        public int ParentPageSize { get; set; }
        public int ParentPageIndex { get; set; }
        public string MaterialNatureFilter { get; set; }
        public int CurrentTransactorTypeFilter { get; set; }
        public int CompanyFilter { get; set; }
        public string IsozigioName { get; set; }

        public PagedList<WarehouseKartelaLine> ListItems { get; set; }
        public async Task OnGetAsync(int transactorId, int? pageIndexKartela, int? pageSizeKartela, string transactorName, string materialNatureFilter, int? companyFilter, int? parentPageIndex, int? parentPageSize)
        {
            LoadFilters();
          
            CompanyFilter = (int)(companyFilter ?? 0);
            MaterialNatureFilter = materialNatureFilter;
            WarehouseItemNatureEnum natureFilterValue = WarehouseItemNatureEnum.WarehouseItemNatureUndefined;

            switch (MaterialNatureFilter)
            {
                case "WarehouseItemNatureUndefined":
                    IsozigioName = "";
                    natureFilterValue = WarehouseItemNatureEnum.WarehouseItemNatureUndefined;
                    break;
                case "WarehouseItemNatureMaterial":
                    IsozigioName = "Υλικών";
                    natureFilterValue = WarehouseItemNatureEnum.WarehouseItemNatureMaterial;
                    break;
                case "WarehouseItemNatureService":
                    IsozigioName = "Υπηρεσιών";
                    natureFilterValue = WarehouseItemNatureEnum.WarehouseItemNatureService;
                    break;
                case "WarehouseItemNatureExpense":
                    IsozigioName = "Δαπάνων";
                    natureFilterValue = WarehouseItemNatureEnum.WarehouseItemNatureExpense;
                    break;
                case "WarehouseItemNatureFixedAsset":
                    IsozigioName = "Πάγια";
                    natureFilterValue = WarehouseItemNatureEnum.WarehouseItemNatureFixedAsset;
                    break;
                default:
                    IsozigioName = "";
                    natureFilterValue = WarehouseItemNatureEnum.WarehouseItemNatureUndefined;
                    break;
            }
           

            ParentPageSize = (int)(parentPageSize ?? 0);
            ParentPageIndex = (int)(parentPageIndex ?? 0);
            TransactorId = transactorId;

            TransactorName = transactorName;
            PageSizeKartela = (int)((pageSizeKartela == null || pageSizeKartela == 0) ? 20 : pageSizeKartela);



            IQueryable<WarehouseTransaction> transactionsList = _context.WarehouseTransactions;
            if (natureFilterValue != WarehouseItemNatureEnum.WarehouseItemNatureUndefined)
            {
                transactionsList = transactionsList.Where(p => p.WarehouseItem.WarehouseItemNature == natureFilterValue);
            }
            if (companyFilter > 0)
            {
                transactionsList = transactionsList.Where(p => p.CompanyId == companyFilter);
            }
            var dbTrans = transactionsList.ProjectTo<WarehouseTransListDto>(_mapper.ConfigurationProvider);

            var dbTransactions = dbTrans.GroupBy(g => new
            {
                g.CompanyCode,
                MaterialName = g.WarehouseItemName,
             
            }
                )
                .Select(s => new
                {
                    MaterialName = s.Key.MaterialName,
                    CompanyCode = s.Key.CompanyCode,
                    ImportVolume = s.Sum(x => x.ImportUnits),
                    ExportVolume = s.Sum(x => x.ExportUnits),
                    ImportValue = s.Sum(x => x.ImportAmount),
                    ExportValue = s.Sum(x => x.ExportAmount)
                }).ToList();

           
            var listWithTotal = new List<WarehouseKartelaLine>();
            decimal runningTotalVolume = 0;
            decimal runningTotalValue = 0;
            foreach (var dbTransaction in dbTransactions)
            {

                runningTotalVolume = dbTransaction.ImportVolume - dbTransaction.ExportVolume ;
                runningTotalValue = dbTransaction.ImportValue - dbTransaction.ExportValue ;
                listWithTotal.Add(new WarehouseKartelaLine
                {

                    CompanyCode = dbTransaction.CompanyCode,
                    RunningTotalVolume = runningTotalVolume,
                    RunningTotalValue = runningTotalValue,
                    MaterialName = dbTransaction.MaterialName,
                    ImportVolume = dbTransaction.ImportVolume,
                    ExportVolume = dbTransaction.ExportVolume,
                    ImportValue = dbTransaction.ImportValue,
                    ExportValue = dbTransaction.ExportValue
                });
            }

            var outList = listWithTotal.AsQueryable();


            IQueryable<WarehouseKartelaLine> fullListIq = from s in outList select s;

            ListItems = PagedList<WarehouseKartelaLine>.Create(
                fullListIq, pageIndexKartela ?? 1, PageSizeKartela);

            foreach (var item in ListItems)
            {
                sumImportsVolume += item.ImportVolume;
                sumExportsVolume += item.ExportVolume;
                sumImportsValue += item.ImportValue;
                sumExportsValue += item.ExportValue;

            }



        }
        private void LoadFilters()
        {
            var materialNatures = Enum.GetValues(typeof(WarehouseItemNatureEnum))
                .Cast<WarehouseItemNatureEnum>()
                .Select(c => new SelectListItem()
                {
                    Value = c.ToString(),
                    Text = c.GetDescription()
                }).ToList();
            //List<SelectListItem> materialNatures = new List<SelectListItem>
            //{
            //    new SelectListItem() {Value = "0", Text = "{All Natures}"},
            //    new SelectListItem() {Value = WarehouseItemNatureEnum.WarehouseItemNatureMaterial.ToString(), Text = "Υλικό"},
            //    new SelectListItem() {Value = WarehouseItemNatureEnum.WarehouseItemNatureService.ToString(), Text = "Υπηρεσία"},
            //    new SelectListItem() {Value = WarehouseItemNatureEnum.WarehouseItemNatureExpense.ToString(), Text = "Δαπάνη"},
            //    new SelectListItem() {Value = WarehouseItemNatureEnum.WarehouseItemNatureFixedAsset.ToString(), Text = "Πάγιο"}
            //};

            ViewData["MaterialNatureValues"] = new SelectList(materialNatures, "Value", "Text");

            var dbCompanies = _context.Companies.OrderBy(p => p.Code).AsNoTracking();
            List<SelectListItem> companiesList = new List<SelectListItem>();
            companiesList.Add(new SelectListItem() { Value = 0.ToString(), Text = "{All Companies}" });
            foreach (var company in dbCompanies)
            {
                companiesList.Add(new SelectListItem() { Value = company.Id.ToString(), Text = company.Code });
            }
            ViewData["CompanyFilter"] = new SelectList(companiesList, "Value", "Text");
        }
    }
}