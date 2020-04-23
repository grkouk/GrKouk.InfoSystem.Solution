﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.MediaEntities;
using GrKouk.InfoSystem.Domain.RecurringTransactions;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using GrKouk.InfoSystem.Dtos.WebDtos;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.CashRegister;
using GrKouk.InfoSystem.Dtos.WebDtos.Diaries;
using GrKouk.InfoSystem.Dtos.WebDtos.DiaryTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.Media;
using GrKouk.InfoSystem.Dtos.WebDtos.ProductRecipies;
using GrKouk.InfoSystem.Dtos.WebDtos.RecurringTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.SellDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.Transactors;
using GrKouk.InfoSystem.Dtos.WebDtos.TransactorTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseItems;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseTransactions;
using GrKouk.WebApi.Data;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace GrKouk.WebRazor.Controllers
{
    public class IdList
    {
        public string Section { get; set; }
        public List<int> Ids { get; set; }
    }

    public class MediaListProductRequest
    {
        public int ProductId { get; set; }
        public List<int> MediaIds { get; set; }
    }
    public class CashCategoriesProductsRequest
    {
        public int ClientProfileId { get; set; }
        public int CashRegCategoryId { get; set; }
        public List<int> ProductIdList { get; set; }

    }

    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class GrkoukInfoApiController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public GrkoukInfoApiController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet("UnlinkProductImages")]
        public async Task<IActionResult> UnlinkProductImages(int ProductId)
        {
            string message = "";
            using (var transaction = _context.Database.BeginTransaction())
            {
                var mediaToDelete = await _context.ProductMedia.Where(p => p.ProductId == ProductId).ToListAsync();
                if (mediaToDelete.Count > 0)
                {
                    foreach (var mediaItem in mediaToDelete)
                    {
                        _context.ProductMedia.Remove(mediaItem);
                    }

                    try
                    {
                        var mediaUnlinked = await _context.SaveChangesAsync();
                        var ms = new StringBuilder()
                            .Append("Unlinking media successful")
                            .Append($"</br>Unlinked: {mediaUnlinked} media");

                        message = ms.ToString();

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        transaction.Rollback();
                        var ms = new StringBuilder()
                            .Append("Unlinking media unsuccessful")
                            .Append($"</br> {e.Message}");
                        message = ms.ToString();

                    }
                }


            }

            return Ok(new { message = message });
        }
        
        [HttpPost("DeleteBuyDocumentsList")]
        public async Task<IActionResult> DeleteBuyDocumentList([FromBody] IdList docIds)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                foreach (var itemId in docIds.Ids)
                {
                    Debug.Write("test");

                }

                transaction.Commit();
            }

            return Ok();
        }
        [HttpPost("DeleteExpenseTransactionList")]
        public async Task<IActionResult> DeleteExpenseTransactionList([FromBody] IdList docIds)
        {
            //Thread.Sleep(1500);
            using (var transaction = _context.Database.BeginTransaction())
            {
                foreach (var itemId in docIds.Ids)
                {
                    var toRemove = _context.FinDiaryTransactions.First(x => x.Id == itemId);
                    if (toRemove != null)
                    {
                        _context.FinDiaryTransactions.Remove(toRemove);
                    }
                }

                try
                {
                    var toDeleteCount = _context.ChangeTracker.Entries().Count(x => x.State == EntityState.Deleted);
                    // throw new Exception("Test error");
                    var deletedCount = await _context.SaveChangesAsync();
                    transaction.Commit();
                    string message = $"Selected:{toDeleteCount}. Actually deleted:{deletedCount} ";
                    return Ok(new { message });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    //throw;
                    return NotFound(new
                    {
                        Error = e.Message
                    });
                }
            }
        }

        [HttpGet("GetIndexTblDataExpenses")]
        public async Task<IActionResult> GetIndexTblDataExpenses([FromQuery] IndexDataTableRequest request)
        {
            IQueryable<FinDiaryTransaction> expensesIq = from s in _context.FinDiaryTransactions
                                                         select s;
            //var q = _context.FinDiaryTransactions.Include(p => p.Company)
            //    .ThenInclude(p => p.Currency)
            //    .ThenInclude(p => p.Rates.OrderByDescending(s => s.ClosingDate));

            expensesIq = expensesIq.Include(f => f.Company)
                .Include(f => f.CostCentre)
                .Include(f => f.FinTransCategory)
                .Include(f => f.Transactor);
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "transactiondate:asc":
                        expensesIq = expensesIq.OrderBy(p => p.TransactionDate);
                        break;
                    case "transactiondate:desc":
                        expensesIq = expensesIq.OrderByDescending(p => p.TransactionDate);
                        break;
                    case "transactorname:asc":
                        expensesIq = expensesIq.OrderBy(p => p.Transactor.Name);
                        break;
                    case "transactorname:desc":
                        expensesIq = expensesIq.OrderByDescending(p => p.Transactor.Name);
                        break;

                }
            }

            if (!String.IsNullOrEmpty(request.DateRange))
            {
                var datePeriodFilter = request.DateRange;
                DateFilterDates dfDates = DateFilter.GetDateFilterDates(datePeriodFilter);
                DateTime fromDate = dfDates.FromDate;
                DateTime toDate = dfDates.ToDate;

                expensesIq = expensesIq.Where(p => p.TransactionDate >= fromDate && p.TransactionDate <= toDate);
            }

            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        expensesIq = expensesIq.Where(p => p.CompanyId == companyId);
                    }

                }


            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                expensesIq = expensesIq.Where(p => p.Transactor.Name.Contains(request.SearchFilter)
                                                   || p.ReferenceCode.Contains(request.SearchFilter)
                                                   || p.FinTransCategory.Name.Contains(request.SearchFilter));
            }

            var t = expensesIq.ProjectTo<FinDiaryExpenseTransactionDto>(_mapper.ConfigurationProvider);
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<FinDiaryExpenseTransactionDto>.CreateAsync(t, pageIndex, pageSize);
            foreach (var listItem in listItems)
            {
                if (listItem.CompanyCurrencyId != 1)
                {
                    var r = await _context.ExchangeRates.Where(p => p.CurrencyId == listItem.CompanyCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefaultAsync();
                    if (r != null)
                    {
                        listItem.AmountFpa /= r.Rate;
                        listItem.AmountNet /= r.Rate;
                        listItem.AmountTotal /= r.Rate;
                    }
                }
                if (request.DisplayCurrencyId != 1)
                {
                    var r = await _context.ExchangeRates.Where(p => p.CurrencyId == request.DisplayCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefaultAsync();
                    if (r != null)
                    {
                        listItem.AmountFpa *= r.Rate;
                        listItem.AmountNet *= r.Rate;
                        listItem.AmountTotal *= r.Rate;
                    }
                }

            }
            decimal sumAmountTotal = listItems.Sum(p => p.AmountTotal);

            var response = new IndexDataTableResponse<FinDiaryExpenseTransactionDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                SumOfAmount = sumAmountTotal,
                Data = listItems
            };
            return Ok(response);
        }

        [HttpGet("GetSelectorTransactorTypes")]
        public async Task<ActionResult<IList<UISelectTypeItem>>> GetSelectorTransactorTypes()
        {
            var transactorTypeList = await _context.TransactorTypes.OrderBy(p => p.Name)
                .Select(p => new UISelectTypeItem()
                {
                    Title = p.Name,
                    ValueInt = p.Id,
                    Value = p.Id.ToString()
                }).ToListAsync();
            return Ok(transactorTypeList);
        }
        [HttpGet("GetSelectorMaterialNatures")]
        public ActionResult<IList<UISelectTypeItem>> GetSelectorMaterialNatures()
        {
            var materialNatureList = Enum.GetValues(typeof(WarehouseItemNatureEnum))
                .Cast<WarehouseItemNatureEnum>()
                .Select(c => new UISelectTypeItem()
                {
                    ValueInt = (int)c,
                    Title = c.GetDescription()
                }).ToList();

            return Ok(materialNatureList);
        }
        [HttpGet("GetSelectorCompanies")]
        public ActionResult<IList<UISelectTypeItem>> GetSelectorCompanies()
        {
            var dbCompanies = _context.Companies.Where(t => t.Id != 1).OrderBy(p => p.Code).AsNoTracking();
            List<UISelectTypeItem> companiesList = new List<UISelectTypeItem>();
            //companiesList.Add(new UISelectTypeItem() { Value = 0.ToString(), Text = "{All Companies}" });
            foreach (var company in dbCompanies)
            {
                companiesList.Add(new UISelectTypeItem() { Value = company.Id.ToString(), Title = company.Code });
            }
            return Ok(companiesList);
        }
        [HttpGet("GetIndexTblDataBuyDocuments")]
        public async Task<IActionResult> GetIndexTblDataBuyDocuments([FromQuery] IndexDataTableRequest request)
        {
            IQueryable<BuyDocument> fullListIq = _context.BuyDocuments;

            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "transactiondatesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.TransDate);
                        break;
                    case "transactiondatesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.TransDate);
                        break;
                    case "transactornamesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Transactor.Name);
                        break;
                    case "transactornamesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Transactor.Name);
                        break;

                    case "seriescodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.BuyDocSeries.Code);
                        break;
                    case "seriescodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.BuyDocSeries.Code);
                        break;
                    case "companycodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Company.Code);
                        break;
                    case "companycodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Company.Code);
                        break;
                }
            }

            if (!String.IsNullOrEmpty(request.DateRange))
            {
                var datePeriodFilter = request.DateRange;
                DateFilterDates dfDates = DateFilter.GetDateFilterDates(datePeriodFilter);
                DateTime fromDate = dfDates.FromDate;
                DateTime toDate = dfDates.ToDate;

                fullListIq = fullListIq.Where(p => p.TransDate >= fromDate && p.TransDate <= toDate);
            }

            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.CompanyId == companyId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.Transactor.Name.Contains(request.SearchFilter)
                                                   || p.TransRefCode.Contains(request.SearchFilter));
            }
            var currencyRates = await _context.ExchangeRates.OrderByDescending(p => p.ClosingDate)
                .Take(10)
                .ToListAsync();
            var t = fullListIq.ProjectTo<BuyDocListDto>(_mapper.ConfigurationProvider);
            var t1 =await t.Select(p => new BuyDocListDto
            {
                Id = p.Id,
                TransDate = p.TransDate,
                TransRefCode = p.TransRefCode,
                SectionId = p.SectionId,
                SectionCode = p.SectionCode,
                TransactorId = p.TransactorId,
                TransactorName = p.TransactorName,
                BuyDocSeriesId = p.BuyDocSeriesId,
                BuyDocSeriesCode = p.BuyDocSeriesCode,
                BuyDocSeriesName = p.BuyDocSeriesName,
                AmountFpa = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountFpa),
                AmountNet = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountNet),
                AmountDiscount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountDiscount),
                CompanyId = p.CompanyId,
                CompanyCode = p.CompanyCode,
                CompanyCurrencyId = p.CompanyCurrencyId
            }).ToListAsync();
            var grandSumOfAmount = t1.Sum(p => p.TotalAmount);
            var gransSumOfNetAmount = t1.Sum(p => p.TotalNetAmount);

            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<BuyDocListDto>.CreateAsync(t, pageIndex, pageSize);
            foreach (var listItem in listItems)
            {
                if (listItem.CompanyCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == listItem.CompanyCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        listItem.AmountFpa /= r.Rate;
                        listItem.AmountNet /= r.Rate;
                        listItem.AmountDiscount /= r.Rate;
                    }
                }
                if (request.DisplayCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == request.DisplayCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        listItem.AmountFpa *= r.Rate;
                        listItem.AmountNet *= r.Rate;
                        listItem.AmountDiscount *= r.Rate;
                    }
                }

            }
            decimal sumAmountTotal = listItems.Sum(p => p.TotalAmount);
            //decimal sumAmountTotal = listItems.Sum(p => p.TotalAmount);
            //decimal sumDebit = listItems.Sum(p => p.DebitAmount);
            //decimal sumCredit = listItems.Sum(p => p.CreditAmount);
            var response = new IndexDataTableResponse<BuyDocListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                SumOfAmount = sumAmountTotal,
                
                GrandSumOfAmount = grandSumOfAmount,
                //GrandSumOfNetAmount = gransSumOfNetAmount,
                Data = listItems
            };
            //return new JsonResult(response);
            return Ok(response);
        }
        [HttpGet("GetIndexTblDataRecurringDocuments")]
        public async Task<IActionResult> GetIndexTblDataRecurringDocuments([FromQuery] IndexDataTableRequest request)
        {
            IQueryable<RecurringTransDoc> fullListIq = _context.RecurringTransDocs;

            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "transactiondatesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.NextTransDate);
                        break;
                    case "transactiondatesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.NextTransDate);
                        break;
                    case "transactornamesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Transactor.Name);
                        break;
                    case "transactornamesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Transactor.Name);
                        break;

                    //case "seriescodesort:asc":
                    //    fullListIq = fullListIq.OrderBy(p => p.BuyDocSeries.Code);
                    //    break;
                    //case "seriescodesort:desc":
                    //    fullListIq = fullListIq.OrderByDescending(p => p.BuyDocSeries.Code);
                    //    break;
                    case "companycodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Company.Code);
                        break;
                    case "companycodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Company.Code);
                        break;
                }
            }

            if (!String.IsNullOrEmpty(request.DateRange))
            {
                var datePeriodFilter = request.DateRange;
                DateFilterDates dfDates = DateFilter.GetRecTransDateFilterDates(datePeriodFilter);
                DateTime fromDate = dfDates.FromDate;
                DateTime toDate = dfDates.ToDate;

               fullListIq = fullListIq.Where(p => p.NextTransDate >= fromDate && p.NextTransDate <= toDate);
            }

            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.CompanyId == companyId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.Transactor.Name.Contains(request.SearchFilter)
                                                   || p.TransRefCode.Contains(request.SearchFilter));
            }
            var currencyRates = await _context.ExchangeRates.OrderByDescending(p => p.ClosingDate)
                .Take(10)
                .ToListAsync();
            var docBuySeries = await _context.BuyDocSeriesDefs.ToListAsync();
            var docSellSeries = await _context.SellDocSeriesDefs.ToListAsync();

            var t = fullListIq.ProjectTo<RecurringDocListDto>(_mapper.ConfigurationProvider);
            var t1 =  t.Select(p => new RecurringDocListDto
            {
                Id = p.Id,
                NextTransDate = p.NextTransDate,
                RecurringDocTypeName=GetRecurringTypeName( p.RecurringDocType),
                RecurringFrequency=p.RecurringFrequency,
                TransRefCode = p.TransRefCode,
                SectionId = p.SectionId,
                SectionCode = p.SectionCode,
                TransactorId = p.TransactorId,
                TransactorName = p.TransactorName,
                DocSeriesId = p.DocSeriesId,
                DocSeriesCode = GetDocSeriesCode(p.RecurringDocType,p.DocSeriesId,docBuySeries,docSellSeries),
                DocSeriesName = GetDocSeriesName(p.RecurringDocType, p.DocSeriesId, docBuySeries, docSellSeries),
                AmountFpa = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountFpa),
                AmountNet = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountNet),
                AmountDiscount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountDiscount),
                CompanyId = p.CompanyId,
                CompanyCode = p.CompanyCode,
                CompanyCurrencyId = p.CompanyCurrencyId
            });
           

            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<RecurringDocListDto>.CreateAsync(t1, pageIndex, pageSize);
            var grandSumOfAmount = t1.Sum(p => p.TotalAmount);
            var gransSumOfNetAmount = t1.Sum(p => p.TotalNetAmount);
            foreach (var listItem in listItems)
            {
                if (listItem.CompanyCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == listItem.CompanyCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        listItem.AmountFpa /= r.Rate;
                        listItem.AmountNet /= r.Rate;
                        listItem.AmountDiscount /= r.Rate;
                    }
                }
                if (request.DisplayCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == request.DisplayCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        listItem.AmountFpa *= r.Rate;
                        listItem.AmountNet *= r.Rate;
                        listItem.AmountDiscount *= r.Rate;
                    }
                }

            }
            decimal sumAmountTotal = listItems.Sum(p => p.TotalAmount);
            //decimal sumAmountTotal = listItems.Sum(p => p.TotalAmount);
            //decimal sumDebit = listItems.Sum(p => p.DebitAmount);
            //decimal sumCredit = listItems.Sum(p => p.CreditAmount);
            var response = new IndexDataTableResponse<RecurringDocListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                SumOfAmount = sumAmountTotal,

                GrandSumOfAmount = grandSumOfAmount,
                //GrandSumOfNetAmount = gransSumOfNetAmount,
                Data = listItems
            };
            //return new JsonResult(response);
            return Ok(response);
        }
        [HttpGet("GetIndexTblDataProductionRecipes")]
        public async Task<IActionResult> GetIndexTblDataProductionRecipes([FromQuery] IndexDataTableRequest request)
        {
            IQueryable<ProductRecipe> fullListIq = _context.ProductRecipes;

            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {

                    case "transactorname:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Product.Name);
                        break;
                    case "transactorname:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Product.Name);
                        break;

                }
            }

            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.CompanyId == companyId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.Product.Name.Contains(request.SearchFilter)
                                                   || p.Company.Name.Contains(request.SearchFilter));
            }
            var t = fullListIq.ProjectTo<ProductRecipeDto>(_mapper.ConfigurationProvider);
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<ProductRecipeDto>.CreateAsync(t, pageIndex, pageSize);
            // decimal sumAmountTotal = listItems.Sum(p => p.TotalAmount);

            var response = new IndexDataTableResponse<ProductRecipeDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                // SumOfAmount = sumAmountTotal,
                Data = listItems
            };
            //return new JsonResult(response);
            return Ok(response);
        }
        [HttpGet("GetIndexTblDataSellDocuments")]
        public async Task<IActionResult> GetIndexTblDataSellDocuments([FromQuery] IndexDataTableRequest request)
        {
            IQueryable<SellDocument> fullListIq = _context.SellDocuments;

            //fullListIq = fullListIq.Include(f => f.Company)
            //    .Include(f => f.CostCentre)
            //    .Include(f => f.FinTransCategory)
            //    .Include(f => f.Transactor);
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "transactiondatesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.TransDate);
                        break;
                    case "transactiondatesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.TransDate);
                        break;
                    case "transactornamesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Transactor.Name);
                        break;
                    case "transactornamesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Transactor.Name);
                        break;
                    case "seriescodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.SellDocSeries.Code);
                        break;
                    case "seriescodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.SellDocSeries.Code);
                        break;
                    case "companycodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Company.Code);
                        break;
                    case "companycodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Company.Code);
                        break;
                }
            }

            if (!String.IsNullOrEmpty(request.DateRange))
            {
                var datePeriodFilter = request.DateRange;
                DateFilterDates dfDates = DateFilter.GetDateFilterDates(datePeriodFilter);
                DateTime fromDate = dfDates.FromDate;
                DateTime toDate = dfDates.ToDate;

                fullListIq = fullListIq.Where(p => p.TransDate >= fromDate && p.TransDate <= toDate);
            }

            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.CompanyId == companyId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.Transactor.Name.Contains(request.SearchFilter)
                                                   || p.TransRefCode.Contains(request.SearchFilter));
            }
            var currencyRates = await _context.ExchangeRates.OrderByDescending(p => p.ClosingDate)
                .Take(10)
                .ToListAsync();
            var t = fullListIq.ProjectTo<SellDocListDto>(_mapper.ConfigurationProvider);
            var t1 =await t.Select(p => new SellDocListDto
            {
                Id = p.Id,
                TransDate = p.TransDate,
                TransRefCode = p.TransRefCode,
                SectionId = p.SectionId,
                SectionCode = p.SectionCode,
                TransactorId = p.TransactorId,
                TransactorName = p.TransactorName,
                SellDocSeriesId = p.SellDocSeriesId,
                SellDocSeriesCode = p.SellDocSeriesCode,
                SellDocSeriesName = p.SellDocSeriesName,
                AmountFpa = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountFpa),
                AmountNet = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountNet),
                AmountDiscount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountDiscount),

                CompanyId = p.CompanyId,
                CompanyCode = p.CompanyCode,
                CompanyCurrencyId = p.CompanyCurrencyId,
                SalesChannelId = p.SalesChannelId
            }).ToListAsync();
            var gransSumOfAmount = t1.Sum(p => p.TotalAmount);
            var gransSumOfNetAmount = t1.Sum(p => p.TotalNetAmount);
            
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<SellDocListDto>.CreateAsync(t, pageIndex, pageSize);
            foreach (var listItem in listItems)
            {
                if (listItem.CompanyCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == listItem.CompanyCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        listItem.AmountFpa /= r.Rate;
                        listItem.AmountNet /= r.Rate;
                        listItem.AmountDiscount /= r.Rate;
                    }
                }
                if (request.DisplayCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == request.DisplayCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        listItem.AmountFpa = listItem.AmountFpa * r.Rate;
                        listItem.AmountNet = listItem.AmountNet * r.Rate;
                        listItem.AmountDiscount = listItem.AmountDiscount * r.Rate;
                    }
                }

            }
            decimal sumAmountTotal = listItems.Sum(p => p.TotalAmount);
            decimal sumAmountNet= listItems.Sum(p => p.TotalNetAmount);
            var response = new IndexDataTableResponse<SellDocListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                SumOfAmount = sumAmountTotal,
                SumOfNetAmount = sumAmountNet,
                GrandSumOfAmount = gransSumOfAmount,
                GrandSumOfNetAmount = gransSumOfNetAmount,
                Data = listItems
            };
            //return new JsonResult(response);
            return Ok(response);
        }
        [HttpGet("GetIndexTblDataTransactorTrans")]
        public async Task<IActionResult> GetIndexTblDataTransactorTrans([FromQuery] IndexDataTableRequest request)
        {
            IQueryable<TransactorTransaction> fullListIq = _context.TransactorTransactions;

            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "transactiondatesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.TransDate);
                        break;
                    case "transactiondatesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.TransDate);
                        break;
                    case "transactornamesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Transactor.Name);
                        break;
                    case "transactornamesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Transactor.Name);
                        break;
                    case "seriescodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.TransTransactorDocSeries.Code);
                        break;
                    case "seriescodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.TransTransactorDocSeries.Code);
                        break;
                    case "companycodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Company.Code);
                        break;
                    case "companycodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Company.Code);
                        break;
                    case "sectioncodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Section.Code);
                        break;
                    case "sectioncodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Section.Code);
                        break;
                }
            }

            if (!String.IsNullOrEmpty(request.DateRange))
            {
                var datePeriodFilter = request.DateRange;
                DateFilterDates dfDates = DateFilter.GetDateFilterDates(datePeriodFilter);
                DateTime fromDate = dfDates.FromDate;
                DateTime toDate = dfDates.ToDate;

                fullListIq = fullListIq.Where(p => p.TransDate >= fromDate && p.TransDate <= toDate);
            }

            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.CompanyId == companyId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.Transactor.Name.Contains(request.SearchFilter)
                                                   || p.TransRefCode.Contains(request.SearchFilter));
            }
            var currencyRates = await _context.ExchangeRates.OrderByDescending(p => p.ClosingDate)
                .Take(10)
                .ToListAsync();
            var t = fullListIq.ProjectTo<TransactorTransListDto>(_mapper.ConfigurationProvider);
            var t1 =await t.Select(p => new TransactorTransListDto
            {
                Id = p.Id,
                TransDate = p.TransDate,
                TransTransactorDocSeriesId = p.TransTransactorDocSeriesId,
                TransTransactorDocSeriesName = p.TransTransactorDocSeriesName,
                TransTransactorDocSeriesCode = p.TransTransactorDocSeriesCode,
                TransTransactorDocTypeId = p.TransTransactorDocTypeId,
                TransRefCode = p.TransRefCode,
                TransactorId = p.TransactorId,
                TransactorName = p.TransactorName,
                SectionId = p.SectionId,
                SectionCode = p.SectionCode,
                CreatorId = p.CreatorId,
                FiscalPeriodId = p.FiscalPeriodId,
                FinancialAction = p.FinancialAction,
                FpaRate = p.FpaRate,
                DiscountRate = p.DiscountRate,
                AmountFpa = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountFpa),
                AmountNet = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountNet),
                AmountDiscount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates,
                    p.AmountDiscount),
                TransFpaAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates,
                    p.TransFpaAmount),
                TransNetAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates,
                    p.TransNetAmount),
                TransDiscountAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates,
                    p.TransDiscountAmount),
                CompanyCode = p.CompanyCode,
                CompanyCurrencyId = p.CompanyCurrencyId
            }).ToListAsync();
            var grandSumOfAmount = t1.Sum(p => p.TotalAmount);
            var grandSumOfDebit = t1.Sum(p => p.DebitAmount);
            var grandSumOfCredit = t1.Sum(p => p.CreditAmount);
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<TransactorTransListDto>.CreateAsync(t, pageIndex, pageSize);
            foreach (var listItem in listItems)
            {
                if (listItem.CompanyCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == listItem.CompanyCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        listItem.AmountFpa = listItem.AmountFpa / r.Rate;
                        listItem.AmountNet = listItem.AmountNet / r.Rate;
                        listItem.AmountDiscount = listItem.AmountDiscount / r.Rate;
                        listItem.TransFpaAmount = listItem.TransFpaAmount / r.Rate;
                        listItem.TransNetAmount = listItem.TransNetAmount / r.Rate;
                        listItem.TransDiscountAmount = listItem.TransDiscountAmount / r.Rate;

                    }
                }
                if (request.DisplayCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == request.DisplayCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        listItem.AmountFpa = listItem.AmountFpa * r.Rate;
                        listItem.AmountNet = listItem.AmountNet * r.Rate;
                        listItem.AmountDiscount = listItem.AmountDiscount * r.Rate;
                        listItem.TransFpaAmount = listItem.TransFpaAmount * r.Rate;
                        listItem.TransNetAmount = listItem.TransNetAmount * r.Rate;
                        listItem.TransDiscountAmount = listItem.TransDiscountAmount * r.Rate;

                    }
                }

            }
            decimal sumAmountTotal = listItems.Sum(p => p.TotalAmount);
            decimal sumDebit = listItems.Sum(p => p.DebitAmount);
            decimal sumCredit = listItems.Sum(p => p.CreditAmount);
            var response = new IndexDataTableResponse<TransactorTransListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                SumOfAmount = sumAmountTotal,
                SumOfDebit = sumDebit,
                SumOfCredit = sumCredit,
                GrandSumOfAmount = grandSumOfAmount,
                GrandSumOfDebit = grandSumOfDebit,
                GrandSumOfCredit = grandSumOfCredit,
                Data = listItems
            };
            //return new JsonResult(response);
            return Ok(response);
        }
        [HttpGet("GetIndexTblDataWarehouseTrans")]
        public async Task<IActionResult> GetIndexTblDataWarehouseTrans([FromQuery] IndexDataTableRequest request)
        {
            IQueryable<WarehouseTransaction> fullListIq = _context.WarehouseTransactions;

            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "transactiondatesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.TransDate);
                        break;
                    case "transactiondatesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.TransDate);
                        break;
                    case "warehouseitemnamesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.WarehouseItem.Name);
                        break;
                    case "warehouseitemnamesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.WarehouseItem.Name);
                        break;
                    case "seriescodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.TransWarehouseDocSeries.Code);
                        break;
                    case "seriescodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.TransWarehouseDocSeries.Code);
                        break;
                    case "companycodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Company.Code);
                        break;
                    case "companycodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Company.Code);
                        break;
                    case "sectioncodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Section.Code);
                        break;
                    case "sectioncodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Section.Code);
                        break;
                }
            }

            if (!String.IsNullOrEmpty(request.DateRange))
            {
                var datePeriodFilter = request.DateRange;
                DateFilterDates dfDates = DateFilter.GetDateFilterDates(datePeriodFilter);
                DateTime fromDate = dfDates.FromDate;
                DateTime toDate = dfDates.ToDate;

                fullListIq = fullListIq.Where(p => p.TransDate >= fromDate && p.TransDate <= toDate);
            }

            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.CompanyId == companyId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.WarehouseItem.Name.Contains(request.SearchFilter)
                                                   || p.WarehouseItem.Code.Contains(request.SearchFilter)
                                                   || p.TransRefCode.Contains(request.SearchFilter));
            }
            var currencyRates = await _context.ExchangeRates.OrderByDescending(p => p.ClosingDate)
                .Take(10)
                .ToListAsync();
            var t = fullListIq.ProjectTo<WarehouseTransListDto>(_mapper.ConfigurationProvider);
            var t1 =await t.Select(p => new WarehouseTransListDto
            {
                Id = p.Id,
                TransDate = p.TransDate,
                TransWarehouseDocSeriesId = p.TransWarehouseDocSeriesId,
                TransWarehouseDocSeriesName = p.TransWarehouseDocSeriesName,
                TransWarehouseDocSeriesCode = p.TransWarehouseDocSeriesCode,
                TransRefCode = p.TransRefCode,
                SectionCode = p.SectionCode,
                CreatorId = p.CreatorId,
                WarehouseItemId = p.WarehouseItemId,
                WarehouseItemName = p.WarehouseItemName,
                TransactionType = p.TransactionType,
                InventoryAction = p.InventoryAction,
                InventoryValueAction = p.InventoryValueAction,
                InvoicedVolumeAction = p.InvoicedVolumeAction,
                InvoicedValueAction = p.InvoicedValueAction,
                Quontity1 = p.Quontity1,
                Quontity2 = p.Quontity2,
                UnitPrice = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.UnitPrice),
                UnitExpenses = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.UnitExpenses),
                UnitPriceFinal = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.UnitPriceFinal),
                AmountFpa = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountFpa),
                AmountNet = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountNet),
                AmountDiscount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountDiscount),
                TransQ1 = p.TransQ1,
                TransQ2 = p.TransQ2,
                TransFpaAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.TransFpaAmount),
                TransNetAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.TransNetAmount),
                TransDiscountAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.TransDiscountAmount),
                CompanyId = p.CompanyId,
                CompanyCode = p.CompanyCode,
                CompanyCurrencyId = p.CompanyCurrencyId
            }).ToListAsync();
            decimal grandSumImportVolume = t1.Sum(p => p.ImportUnits);
            decimal grandSumImportValue = t1.Sum(p => p.ImportAmount);
            decimal grandSumExportVolume = t1.Sum(p => p.ExportUnits);
            decimal grandSumExportValue = t1.Sum(p => p.ExportAmount);
           
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<WarehouseTransListDto>.CreateAsync(t, pageIndex, pageSize);
            foreach (var listItem in listItems)
            {
                if (listItem.CompanyCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == listItem.CompanyCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        listItem.AmountFpa = listItem.AmountFpa / r.Rate;
                        listItem.AmountNet = listItem.AmountNet / r.Rate;
                        listItem.AmountDiscount = listItem.AmountDiscount / r.Rate;
                        listItem.TransFpaAmount = listItem.TransFpaAmount / r.Rate;
                        listItem.TransNetAmount = listItem.TransNetAmount / r.Rate;
                        listItem.TransDiscountAmount = listItem.TransDiscountAmount / r.Rate;
                    }
                }
                if (request.DisplayCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == request.DisplayCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        listItem.AmountFpa = listItem.AmountFpa * r.Rate;
                        listItem.AmountNet = listItem.AmountNet * r.Rate;
                        listItem.AmountDiscount = listItem.AmountDiscount * r.Rate;
                        listItem.TransFpaAmount = listItem.TransFpaAmount * r.Rate;
                        listItem.TransNetAmount = listItem.TransNetAmount * r.Rate;
                        listItem.TransDiscountAmount = listItem.TransDiscountAmount * r.Rate;
                    }
                }

            }

            decimal sumImportVolume = listItems.Sum(p => p.ImportUnits);
            decimal sumImportValue = listItems.Sum(p => p.ImportAmount);
            decimal sumExportVolume = listItems.Sum(p => p.ExportUnits);
            decimal sumExportValue = listItems.Sum(p => p.ExportAmount);
            var response = new IndexDataTableResponse<WarehouseTransListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                SumImportVolume = sumImportVolume,
                SumImportValue = sumImportValue,
                SumExportVolume = sumExportVolume,
                SumExportValue = sumExportValue,
                GrandSumImportVolume = grandSumImportVolume,
                GrandSumImportValue = grandSumImportValue,
                GrandSumExportVolume = grandSumExportVolume,
                GrandSumExportValue = grandSumExportValue,
                Data = listItems
            };

            return Ok(response);
        }
        [HttpGet("GetIndexTblDataTransactorsBalance")]
        public async Task<IActionResult> GetIndexTblDataTransactorsBalance([FromQuery] IndexDataTableRequest request)
        {
            IQueryable<TransactorTransaction> transactionsList = _context.TransactorTransactions;
            int transactorTypeId = 0;
            if (!String.IsNullOrEmpty(request.TransactorTypeFilter))
            {

                if (Int32.TryParse(request.TransactorTypeFilter, out transactorTypeId))
                {
                    if (transactorTypeId > 0)
                    {
                        transactionsList = transactionsList.Where(p => p.Transactor.TransactorTypeId == transactorTypeId);
                    }
                }
            }

            #region CommentOut
            //if (!String.IsNullOrEmpty(request.DateRange))
            //{
            //    var datePeriodFilter = request.DateRange;
            //    DateFilterDates dfDates = DateFilter.GetDateFilterDates(datePeriodFilter);
            //    DateTime fromDate = dfDates.FromDate;
            //    DateTime toDate = dfDates.ToDate;

            //    fullListIq = fullListIq.Where(p => p.TransDate >= fromDate && p.TransDate <= toDate);
            //}
            #endregion
            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        transactionsList = transactionsList.Where(p => p.CompanyId == companyId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                transactionsList = transactionsList.Where(p => p.Transactor.Name.Contains(request.SearchFilter));
            }
            var dbTrans = transactionsList.ProjectTo<TransactorTransListDto>(_mapper.ConfigurationProvider);

            var dbTransactions = dbTrans.GroupBy(g => new
            {


                g.CompanyCode,
                g.CompanyCurrencyId,
                g.TransactorId
            }
                )
                .Select(s => new TransactorIsozygioItem
                {
                    Id = s.Key.TransactorId,
                    TransactorName = "",
                    CompanyCode = s.Key.CompanyCode,
                    CompanyCurrencyId = s.Key.CompanyCurrencyId,
                    DebitAmount = s.Sum(x => x.DebitAmount),
                    CreditAmount = s.Sum(x => x.CreditAmount)
                }).ToList();
            foreach (var listItem in dbTransactions)
            {

                if (listItem.CompanyCurrencyId != 1)
                {
                    var r = await _context.ExchangeRates.Where(p => p.CurrencyId == listItem.CompanyCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefaultAsync();
                    if (r != null)
                    {
                        listItem.DebitAmount /= r.Rate;
                        listItem.CreditAmount /= r.Rate;

                    }
                }
                if (request.DisplayCurrencyId != 1)
                {
                    var r = await _context.ExchangeRates.Where(p => p.CurrencyId == request.DisplayCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefaultAsync();
                    if (r != null)
                    {
                        listItem.DebitAmount *= r.Rate;
                        listItem.CreditAmount *= r.Rate;

                    }
                }

            }
            var isozigioType = "FREE";
            var transactorType = await _context.TransactorTypes.Where(c => c.Id == transactorTypeId).FirstOrDefaultAsync();
            var isozigioName = "";
            if (transactorType != null)
            {
                switch (transactorType.Code)
                {
                    case "SYS.DTRANSACTOR":
                        isozigioName = "Συναλλασόμενων Ημερολογίου";
                        isozigioType = "SUPPLIER";
                        break;
                    case "SYS.CUSTOMER":
                        isozigioName = "Πελατών";
                        isozigioType = "CUSTOMER";
                        break;
                    case "SYS.SUPPLIER":
                        isozigioName = "Προμηθευτών";
                        isozigioType = "SUPPLIER";
                        break;

                }

            }
            var listWithTotal = new List<KartelaLine>();

            decimal runningTotal = 0;
            foreach (var dbTransaction in dbTransactions)
            {
                switch (isozigioType)
                {
                    case "SUPPLIER":
                        runningTotal = dbTransaction.CreditAmount - dbTransaction.DebitAmount;
                        break;
                    case "CUSTOMER":
                        runningTotal = dbTransaction.DebitAmount - dbTransaction.CreditAmount;
                        break;
                    default:
                        runningTotal = dbTransaction.CreditAmount - dbTransaction.DebitAmount;
                        break;
                }
                var transactor = await _context.Transactors.Where(c => c.Id == dbTransaction.Id).FirstOrDefaultAsync();
                string transName = "";

                if (transactor != null)
                {
                    transName = transactor.Name;
                }
                listWithTotal.Add(new KartelaLine
                {
                    Id = dbTransaction.Id,
                    RunningTotal = runningTotal,
                    TransactorName = transName,
                    CompanyCode = dbTransaction.CompanyCode,
                    Debit = dbTransaction.DebitAmount,
                    Credit = dbTransaction.CreditAmount
                });
            }

            var outList = listWithTotal.AsQueryable();
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "companysort:asc":
                        outList = outList.OrderBy(p => p.CompanyCode);
                        break;
                    case "companysort:desc":
                        outList = outList.OrderByDescending(p => p.CompanyCode);
                        break;
                    case "namesort:asc":
                        outList = outList.OrderBy(p => p.TransactorName);
                        break;
                    case "namesort:desc":
                        outList = outList.OrderByDescending(p => p.TransactorName);
                        break;

                }
            }
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;
            decimal sumCredit = 0;
            decimal sumDebit = 0;
            decimal sumDifference = 0;

            IQueryable<KartelaLine> fullListIq = from s in outList select s;

            var listItems = PagedList<KartelaLine>.Create(fullListIq, pageIndex, pageSize);

            foreach (var item in listItems)
            {
                sumCredit += item.Credit;
                sumDebit += item.Debit;
            }
            switch (isozigioType)
            {
                case "SUPPLIER":
                    sumDifference = sumCredit - sumDebit;
                    break;
                case "CUSTOMER":
                    sumDifference = sumDebit - sumCredit;
                    break;
                default:
                    sumDifference = sumCredit - sumDebit;
                    break;
            }
            var response = new IndexDataTableResponse<KartelaLine>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                SumOfDebit = sumDebit,
                SumOfCredit = sumCredit,
                SumOfDifference = sumDifference,
                Data = listItems
            };
            return Ok(response);
        }
        [HttpGet("GetIndexTblDataWarehouseBalance")]
        public async Task<IActionResult> GetIndexTblDataWarehouseBalance([FromQuery] IndexDataTableRequest request)
        {
            IQueryable<WarehouseTransaction> transactionsList = _context.WarehouseTransactions;
            WarehouseItemNatureEnum warehouseItemNatureFilter = 0;
            if (!String.IsNullOrEmpty(request.WarehouseItemNatureFilter))
            {
                if (Enum.TryParse(request.WarehouseItemNatureFilter, out warehouseItemNatureFilter))
                {
                    if (warehouseItemNatureFilter > 0)
                    {
                        transactionsList = transactionsList.Where(p => p.WarehouseItem.WarehouseItemNature == warehouseItemNatureFilter);
                    }
                }
            }

            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        transactionsList = transactionsList.Where(p => p.CompanyId == companyId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                transactionsList = transactionsList.Where(p => p.WarehouseItem.Name.Contains(request.SearchFilter));
            }
            var dbTrans = transactionsList.ProjectTo<WarehouseTransListDto>(_mapper.ConfigurationProvider);

            var dbTransactions = dbTrans.GroupBy(g => new
            {
                g.CompanyId,
                g.CompanyCode,
                g.CompanyCurrencyId,
                g.WarehouseItemId
            }
                )
                .Select(s => new WarehouseIsozygioItem
                {
                    Id = s.Key.WarehouseItemId,
                    MaterialName = "",
                    CompanyId = s.Key.CompanyId,
                    CompanyCode = s.Key.CompanyCode,
                    CompanyCurrencyId = s.Key.CompanyCurrencyId,
                    ImportVolume = s.Sum(x => x.ImportUnits),
                    ExportVolume = s.Sum(x => x.ExportUnits),
                    ImportValue = s.Sum(x => x.ImportAmount),
                    ExportValue = s.Sum(x => x.ExportAmount)
                }).ToList();
            foreach (var listItem in dbTransactions)
            {

                if (listItem.CompanyCurrencyId != 1)
                {
                    var r = await _context.ExchangeRates.Where(p => p.CurrencyId == listItem.CompanyCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefaultAsync();
                    if (r != null)
                    {
                        listItem.ImportValue /= r.Rate;
                        listItem.ExportValue /= r.Rate;

                    }
                }
                if (request.DisplayCurrencyId != 1)
                {
                    var r = await _context.ExchangeRates.Where(p => p.CurrencyId == request.DisplayCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefaultAsync();
                    if (r != null)
                    {
                        listItem.ImportValue *= r.Rate;
                        listItem.ExportValue *= r.Rate;

                    }
                }

            }
            //var isozigioType = "FREE";
            //var isozigioName = "";


            switch (warehouseItemNatureFilter)
            {
                case WarehouseItemNatureEnum.WarehouseItemNatureUndefined:
                    //isozigioName = "";
                    break;
                case WarehouseItemNatureEnum.WarehouseItemNatureMaterial:
                    //isozigioName = "Υλικών";
                    //isozigioType = "SUPPLIER";
                    break;
                case WarehouseItemNatureEnum.WarehouseItemNatureService:
                    //isozigioName = "Υπηρεσιών";
                    // isozigioType = "SUPPLIER";
                    break;
                case WarehouseItemNatureEnum.WarehouseItemNatureExpense:
                    //isozigioName = "Δαπάνων";
                    //isozigioType = "SUPPLIER";
                    break;
                case WarehouseItemNatureEnum.WarehouseItemNatureIncome:
                    //isozigioName = "Εσόδων";
                    //isozigioType = "SUPPLIER";
                    break;
                case WarehouseItemNatureEnum.WarehouseItemNatureFixedAsset:
                    //isozigioName = "Παγίων";
                    //isozigioType = "SUPPLIER";
                    break;
                case WarehouseItemNatureEnum.WarehouseItemNatureRawMaterial:
                    //isozigioName = "Πρώτων Υλών";
                    //isozigioType = "SUPPLIER";
                    break;
                default:
                    //isozigioName = "";
                    break;
            }


            var listWithTotal = new List<WarehouseKartelaLine>();
            decimal runningTotalVolume = 0;
            decimal runningTotalValue = 0;
            foreach (var dbTransaction in dbTransactions)
            {

                runningTotalVolume = dbTransaction.ImportVolume - dbTransaction.ExportVolume;
                runningTotalValue = dbTransaction.ImportValue - dbTransaction.ExportValue;
                var warehouseItem = await _context.WarehouseItems.Where(c => c.Id == dbTransaction.Id).FirstOrDefaultAsync();
                string itemName = "";

                if (warehouseItem != null)
                {
                    itemName = warehouseItem.Name;
                }
                listWithTotal.Add(new WarehouseKartelaLine
                {

                    CompanyCode = dbTransaction.CompanyCode,
                    RunningTotalVolume = runningTotalVolume,
                    RunningTotalValue = runningTotalValue,
                    MaterialName = itemName,
                    ImportVolume = dbTransaction.ImportVolume,
                    ExportVolume = dbTransaction.ExportVolume,
                    ImportValue = dbTransaction.ImportValue,
                    ExportValue = dbTransaction.ExportValue
                });
            }



            var outList = listWithTotal.AsQueryable();
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "companysort:asc":
                        outList = outList.OrderBy(p => p.CompanyCode);
                        break;
                    case "companysort:desc":
                        outList = outList.OrderByDescending(p => p.CompanyCode);
                        break;
                    case "datesort:asc":
                        outList = outList.OrderBy(p => p.TransDate);
                        break;
                    case "datesort:desc":
                        outList = outList.OrderByDescending(p => p.TransDate);
                        break;
                    case "namesort:asc":
                        outList = outList.OrderBy(p => p.MaterialName);
                        break;
                    case "namesort:desc":
                        outList = outList.OrderByDescending(p => p.MaterialName);
                        break;

                }
            }
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;
            decimal sumImportsVolume = 0;
            decimal sumExportsVolume = 0;
            decimal sumImportsValue = 0;
            decimal sumExportsValue = 0;
            // decimal sumDifference = 0;

            IQueryable<WarehouseKartelaLine> fullListIq = from s in outList select s;

            var listItems = PagedList<WarehouseKartelaLine>.Create(fullListIq, pageIndex, pageSize);

            foreach (var item in listItems)
            {
                sumImportsVolume += item.ImportVolume;
                sumExportsVolume += item.ExportVolume;
                sumImportsValue += item.ImportValue;
                sumExportsValue += item.ExportValue;
            }

            var response = new IndexDataTableResponse<WarehouseKartelaLine>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                SumImportValue = sumImportsValue,
                SumExportValue = sumExportsValue,
                SumImportVolume = sumImportsVolume,
                SumExportVolume = sumExportsVolume,
                Data = listItems
            };
            return Ok(response);
        }

        [HttpGet("GetIndexTblDataTransactors")]
        public async Task<IActionResult> GetIndexTblDataTransactors([FromQuery] IndexDataTableRequest request)
        {
            var fullListIq = _context.TransactorCompanyMappings
                .Select(t => new TransactorBigClass
                {
                    Id = t.Transactor.Id,
                    Code = t.Transactor.Code,
                    Name = t.Transactor.Name,
                    Address = string.IsNullOrEmpty(t.Transactor.Address) ? " " : t.Transactor.Address,
                    City = string.IsNullOrEmpty(t.Transactor.City) ? " " : t.Transactor.City,
                    EMail = string.IsNullOrEmpty(t.Transactor.EMail) ? " " : t.Transactor.EMail,
                    Zip = t.Transactor.Zip,
                    PhoneWork = string.IsNullOrEmpty(t.Transactor.PhoneWork) ? " " : t.Transactor.PhoneWork,
                    PhoneMobile = string.IsNullOrEmpty(t.Transactor.PhoneMobile) ? " " : t.Transactor.PhoneMobile,
                    PhoneFax = string.IsNullOrEmpty(t.Transactor.PhoneFax) ? " " : t.Transactor.PhoneFax,
                    TransactorTypeId = t.Transactor.TransactorTypeId,
                    TransactorTypeCode = t.Transactor.TransactorType.Code,
                    TransactorTypeName = t.Transactor.TransactorType.Name,
                    CompanyId = t.Company.Id,
                    CompanyCode = t.Company.Code
                });


            int transactorTypeId = 0;
            if (!String.IsNullOrEmpty(request.TransactorTypeFilter))
            {
                if (Int32.TryParse(request.TransactorTypeFilter, out  transactorTypeId))
                {
                    if (transactorTypeId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.TransactorTypeId == transactorTypeId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "transactorcodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Code);
                        break;
                    case "transactorcodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Code);
                        break;
                    case "transactornamesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Name);
                        break;
                    case "transactornamesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Name);
                        break;
                }
            }

            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        var allCompCode =
                            await _context.AppSettings.SingleOrDefaultAsync(
                                p => p.Code == Constants.AllCompaniesCodeKey);
                        if (allCompCode==null)
                        {
                            return NotFound("All Companies Code Setting not found");
                        }
                        var allCompaniesEntity = await _context.Companies.SingleOrDefaultAsync(s => s.Code == allCompCode.Value);
                        if (allCompaniesEntity != null)
                        {
                            var allCompaniesId = allCompaniesEntity.Id;
                            fullListIq = fullListIq.Where(t => t.CompanyId == companyId || t.CompanyId == allCompaniesId);
                        }
                        else
                        {
                            fullListIq = fullListIq.Where(t => t.CompanyId == companyId);
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.Name.Contains(request.SearchFilter)
                                                   || p.EMail.Contains(request.SearchFilter)
                                                   || p.Code.Contains(request.SearchFilter)
                                                   || p.Address.Contains(request.SearchFilter)
                                                   || p.City.Contains(request.SearchFilter)
                                                   || p.PhoneFax.Contains(request.SearchFilter)
                                                   || p.PhoneMobile.Contains(request.SearchFilter)
                                                   || p.PhoneWork.Contains(request.SearchFilter));
            }

            var testList = fullListIq.ToList();
            var projectedList = testList.GroupBy(g => new
            {
                g.Id,
                g.Name,
                g.Code,
                g.EMail,
                g.TransactorTypeCode
            })
                .Select(f => new TransactorListDto
                {
                    Id = f.Key.Id,
                    Name = f.Key.Name,
                    Code = f.Key.Code,
                    TransactorTypeCode = f.Key.TransactorTypeCode,
                    EMail = string.IsNullOrEmpty(f.Key.EMail) ? " " : f.Key.EMail,
                    CompanyCode = String.Join(",", f.Select(n => n.CompanyCode))
                });
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;
            var listItems = PagedList<TransactorListDto>.Create(projectedList.AsQueryable(), pageIndex, pageSize);
            var relevantDiarys = new List<SearchListItem>();
            var dList = await _context.DiaryDefs.Where(p => p.DiaryType == DiaryTypeEnum.DiaryTypeEnumTransactors)
                .ToListAsync();
            if (transactorTypeId != 0)
            {
                var tf = transactorTypeId;
                relevantDiarys = dList
                    .Where(tx => Array.ConvertAll(tx.SelectedTransTypes.Split(","), int.Parse).Contains(tf))
                    .Select(item => new SearchListItem()
                    {
                        Value = item.Id,
                        Text = item.Name
                    })
                    .ToList();
            }

            var response = new IndexDataTableResponse<TransactorListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                Diaries = relevantDiarys,
                Data = listItems
            };
            return Ok(response);
        }
        [HttpGet("GetIndexTblDataWarehouseCodeItems")]
        public async Task<IActionResult> GetIndexTblDataWarehouseCodeItems([FromQuery] IndexDataTableRequest request)
        {
            //Thread.Sleep(10000);
            IQueryable<WrItemCode> fullListIq = _context.WrItemCodes.Include(p=>p.WarehouseItem);
           
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {

                    case "productnamesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.WarehouseItem.Name);
                        break;
                    case "productnamesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.WarehouseItem.Name);
                        break;
                    case "productcodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Code);
                        break;
                    case "productcodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Code);
                        break;
                   
                    default:
                       fullListIq = fullListIq.OrderBy(p => p.Id);
                       break;
                }
            }

            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        var allCompCode =
                            await _context.AppSettings.SingleOrDefaultAsync(
                                p => p.Code == Constants.AllCompaniesCodeKey);
                        if (allCompCode == null)
                        {
                            return NotFound("All Companies Code Setting not found");
                        }
                        var allCompaniesEntity = await _context.Companies.SingleOrDefaultAsync(s => s.Code == allCompCode.Value);
                        if (allCompaniesEntity != null)
                        {
                            var allCompaniesId = allCompaniesEntity.Id;
                            fullListIq = fullListIq.Where(p => p.CompanyId == companyId || p.CompanyId == allCompaniesId);
                        }
                        else
                        {
                            fullListIq = fullListIq.Where(p => p.CompanyId == companyId);
                        }


                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.WarehouseItem.Name.Contains(request.SearchFilter)
                                                   || p.Code.Contains(request.SearchFilter)
                                                   );
            }

            PagedList<WrItemCodeListDto> listItems;
            try
            {
                var projectedList = fullListIq.ProjectTo<WrItemCodeListDto>(_mapper.ConfigurationProvider);
                var pageIndex = request.PageIndex;

                var pageSize = request.PageSize;

                listItems = await PagedList<WrItemCodeListDto>.CreateAsync(projectedList, pageIndex, pageSize);
                foreach (var item in listItems)
                {
                    if (item.CompanyId>0)
                    {
                        var com = await _context.Companies.FirstOrDefaultAsync(p => p.Id == item.CompanyId);
                        if (com !=null)
                        {
                            item.CompanyCode = com.Code;
                            item.CompanyName = com.Name;
                        }
                        else
                        {
                            item.CompanyCode = "##Err##";
                            item.CompanyName = "##Err##";
                        }
                    }
                    else
                    {
                        item.CompanyCode = "{All}";
                        item.CompanyName = "{All Companies}";
                    }
                    if (item.TransactorId > 0)
                    {
                        var com = await _context.Transactors.FirstOrDefaultAsync(p => p.Id == item.TransactorId);
                        if (com != null)
                        {
                            item.TransactorName = com.Name;
                        }
                        else
                        {
                            
                            item.TransactorName = "##Err##";
                        }
                    }
                    else
                    {
                        item.TransactorName = "{All Transactors}";
                    }

                    item.CodeTypeName = item.CodeType.GetDescription();
                    item.CodeUsedUnitName = item.CodeUsedUnit.GetDescription();
                    item.BuyCodeUsedUnitName = item.BuyCodeUsedUnit.GetDescription();
                    item.SellCodeUsedUnitName = item.SellCodeUsedUnit.GetDescription();
                }
            }
            catch (Exception e)
            {
                string msg = e.InnerException.Message;
                return BadRequest(new
                {
                    error = e.Message + " " + msg
                });

            }
            

            var response = new IndexDataTableResponse<WrItemCodeListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                // Diaries = relevantDiarys,
                Data = listItems
            };
            return Ok(response);
        }
        [HttpGet("GetIndexTblDataWarehouseItems")]
        public async Task<IActionResult> GetIndexTblDataWarehouseItems([FromQuery] IndexDataTableRequest request)
        {
            //Thread.Sleep(10000);
            IQueryable<WarehouseItem> fullListIq = _context.WarehouseItems;
            if (!String.IsNullOrEmpty(request.WarehouseItemNatureFilter))
            {
                if (Int32.TryParse(request.WarehouseItemNatureFilter, out var warehouseItemNatureFilter))
                {
                    if (warehouseItemNatureFilter > 0)
                    {
                        var flt = (WarehouseItemNatureEnum)warehouseItemNatureFilter;
                        fullListIq = fullListIq.Where(p => p.WarehouseItemNature == flt);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {

                    case "namesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Name);
                        break;
                    case "namesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Name);
                        break;
                    case "codesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Code);
                        break;
                    case "codesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Code);
                        break;
                    case "categorysort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.MaterialCaterory.Name);
                        break;
                    case "categorysort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.MaterialCaterory.Name);
                        break;
                    case "companycodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Company.Code);
                        break;
                    case "companycodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Company.Code);
                        break;
                }
            }

            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        var allCompCode =
                            await _context.AppSettings.SingleOrDefaultAsync(
                                p => p.Code == Constants.AllCompaniesCodeKey);
                        if (allCompCode == null)
                        {
                            return NotFound("All Companies Code Setting not found");
                        }
                        var allCompaniesEntity = await _context.Companies.SingleOrDefaultAsync(s => s.Code == allCompCode.Value);
                        if (allCompaniesEntity != null)
                        {
                            var allCompaniesId = allCompaniesEntity.Id;
                            fullListIq = fullListIq.Where(p => p.CompanyId == companyId || p.CompanyId == allCompaniesId);
                        }
                        else
                        {
                            fullListIq = fullListIq.Where(p => p.CompanyId == companyId);
                        }


                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.Name.Contains(request.SearchFilter)
                                                   || p.Code.Contains(request.SearchFilter)
                                                   || p.ShortDescription.Contains(request.SearchFilter)
                                                   || p.Description.Contains(request.SearchFilter)
                                                   || p.MaterialCaterory.Name.Contains(request.SearchFilter));
            }

            PagedList<WarehouseItemListDto> listItems;
            try
            {
                var projectedList = fullListIq.ProjectTo<WarehouseItemListDto>(_mapper.ConfigurationProvider);
                var pageIndex = request.PageIndex;

                var pageSize = request.PageSize;

                listItems = await PagedList<WarehouseItemListDto>.CreateAsync(projectedList, pageIndex, pageSize);
            }
            catch (Exception e)
            {
                string msg = e.InnerException.Message;
                return BadRequest(new
                {
                    error = e.Message + " " + msg
                });

            }
            foreach (var productItem in listItems)
            {

                ProductMedia productMedia;

                try
                {
                    productMedia = await _context.ProductMedia
                        .Include(p => p.MediaEntry)
                        .SingleOrDefaultAsync(p => p.ProductId == productItem.Id);
                    if (productMedia != null)
                    {
                        productItem.Url = Url.Content("~/productimages/" + productMedia.MediaEntry.MediaFile);
                    }
                    else
                    {
                        productItem.Url = Url.Content("~/productimages/" + "noimage.jpg");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    productItem.Url = Url.Content("~/productimages/" + "noimage.jpg");
                }

            }

            var response = new IndexDataTableResponse<WarehouseItemListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                // Diaries = relevantDiarys,
                Data = listItems
            };
            return Ok(response);
        }
        [HttpGet("GetSelectorTransactors")]
        public async Task<IActionResult> GetSelectorTransactors([FromQuery] IndexDataTableRequest request)
        {
            var fullListIq = _context.TransactorCompanyMappings
               .Select(t => new TransactorBigClass
               {
                   Id = t.Transactor.Id,
                   Code = t.Transactor.Code,
                   Name = t.Transactor.Name,
                   Address = string.IsNullOrEmpty(t.Transactor.Address) ? " " : t.Transactor.Address,
                   City = string.IsNullOrEmpty(t.Transactor.City) ? " " : t.Transactor.City,
                   EMail = string.IsNullOrEmpty(t.Transactor.EMail) ? " " : t.Transactor.EMail,
                   Zip = t.Transactor.Zip,
                   PhoneWork = string.IsNullOrEmpty(t.Transactor.PhoneWork) ? " " : t.Transactor.PhoneWork,
                   PhoneMobile = string.IsNullOrEmpty(t.Transactor.PhoneMobile) ? " " : t.Transactor.PhoneMobile,
                   PhoneFax = string.IsNullOrEmpty(t.Transactor.PhoneFax) ? " " : t.Transactor.PhoneFax,
                   TransactorTypeId = t.Transactor.TransactorTypeId,
                   TransactorTypeCode = t.Transactor.TransactorType.Code,
                   TransactorTypeName = t.Transactor.TransactorType.Name,
                   CompanyId = t.Company.Id,
                   CompanyCode = t.Company.Code
               });


            int transactorTypeId = 0;
            if (!String.IsNullOrEmpty(request.TransactorTypeFilter))
            {
                if (Int32.TryParse(request.TransactorTypeFilter, out transactorTypeId))
                {
                    if (transactorTypeId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.TransactorTypeId == transactorTypeId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "transactornamesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Name);
                        break;
                    case "transactornamesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Name);
                        break;
                    case "transactorcodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Code);
                        break;
                    case "transactorcodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Code);
                        break;
                    case "companycodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.CompanyCode);
                        break;
                    case "companycodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.CompanyCode);
                        break;
                }
            }
            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {

                var tagIds = request.CompanyFilter.Split(',')
                    .Where(m => int.TryParse(m, out int _))
                    .Select(s => int.Parse(s))
                    .ToList();
                // var companiesList = Array.ConvertAll(request.CompanyFilter.Split(","), int.Parse);
                var allCompCode =
                    await _context.AppSettings.SingleOrDefaultAsync(
                        p => p.Code == Constants.AllCompaniesCodeKey);
                if (allCompCode == null)
                {
                    return NotFound("All Companies Code Setting not found");
                }
                var allCompaniesEntity = await _context.Companies.SingleOrDefaultAsync(s => s.Code == allCompCode.Value);

                if (allCompaniesEntity != null)
                {
                    var allCompaniesId = allCompaniesEntity.Id;
                    tagIds.Add(allCompaniesId);
                }

                fullListIq = fullListIq.Where(p => tagIds.Contains(p.CompanyId));
            }

            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.Name.Contains(request.SearchFilter)
                                                   || p.EMail.Contains(request.SearchFilter)
                                                   || p.Code.Contains(request.SearchFilter)
                                                   || p.Address.Contains(request.SearchFilter)
                                                   || p.City.Contains(request.SearchFilter)
                                                   || p.PhoneFax.Contains(request.SearchFilter)
                                                   || p.PhoneMobile.Contains(request.SearchFilter)
                                                   || p.PhoneWork.Contains(request.SearchFilter));
            }

            var testList = fullListIq.ToList();
            var projectedList = testList.GroupBy(g => new
            {
                g.Id,
                g.Name,
                g.Code,
                g.EMail,
                g.TransactorTypeCode
            })
                .Select(f => new TransactorListDto
                {
                    Id = f.Key.Id,
                    Name = f.Key.Name,
                    Code = f.Key.Code,
                    TransactorTypeCode = f.Key.TransactorTypeCode,
                    EMail = string.IsNullOrEmpty(f.Key.EMail) ? " " : f.Key.EMail,
                    CompanyCode = String.Join(",", f.Select(n => n.CompanyCode))
                });
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;
            var listItems = PagedList<TransactorListDto>.Create(projectedList.AsQueryable(), pageIndex, pageSize);
            var response = new IndexDataTableResponse<TransactorListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
               // Diaries = relevantDiarys,
                Data = listItems
            };
            return Ok(response);
        }

        [HttpGet("GetSelectorWareHouseItems")]
        public async Task<IActionResult> GetSelectorWareHouseItems([FromQuery] IndexDataTableRequest request)
        {
            //Thread.Sleep(10000);
            IQueryable<WarehouseItem> fullListIq = _context.WarehouseItems;

            if (!String.IsNullOrEmpty(request.WarehouseItemNatureFilter))
            {
                try
                {
                    var naturesList = request.WarehouseItemNatureFilter.Split(',')
                        .Where(m => int.TryParse(m, out int _))
                        .Select(m => int.Parse(m))
                        .ToList();
                    //var naturesList = Array.ConvertAll(request.WarehouseItemNatureFilter.Split(","), int.Parse);
                    if (naturesList.Count > 0)
                    {
                        fullListIq = fullListIq.Where(p => naturesList.Contains((int)p.WarehouseItemNature));
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                }

            }
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {

                    case "productnamesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Name);
                        break;
                    case "productnamesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Name);
                        break;
                    case "productcodesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Code);
                        break;
                    case "productcodesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Code);
                        break;
                    case "companynamesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.MaterialCaterory.Name);
                        break;
                    case "companynamesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.MaterialCaterory.Name);
                        break;
                }
            }

            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {

                var tagIds = new List<int>(request.CompanyFilter.Split(',').Select(s => int.Parse(s)));
                // var companiesList = Array.ConvertAll(request.CompanyFilter.Split(","), int.Parse);
                var allCompCode =
                    await _context.AppSettings.SingleOrDefaultAsync(
                        p => p.Code == Constants.AllCompaniesCodeKey);
                if (allCompCode == null)
                {
                    return NotFound("All Companies Code Setting not found");
                }
                var allCompaniesEntity = await _context.Companies.SingleOrDefaultAsync(s => s.Code == allCompCode.Value);

                if (allCompaniesEntity != null)
                {
                    var allCompaniesId = allCompaniesEntity.Id;
                    tagIds.Add(allCompaniesId);
                }

                fullListIq = fullListIq.Where(p => tagIds.Contains(p.CompanyId));
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.Name.Contains(request.SearchFilter)
                                                   || p.Code.Contains(request.SearchFilter)
                                                   || p.WarehouseItemCodes.Any(t=>t.Code==request.SearchFilter)
                                                  // || p.ShortDescription.Contains(request.SearchFilter)
                                                  // || p.Description.Contains(request.SearchFilter)
                                                   //|| p.MaterialCaterory.Name.Contains(request.SearchFilter)
                                                   );
            }

            PagedList<WarehouseItemListDto> listItems;
            try
            {
                var projectedList = fullListIq.ProjectTo<WarehouseItemListDto>(_mapper.ConfigurationProvider);
                var pageIndex = request.PageIndex;

                var pageSize = request.PageSize;

                listItems = await PagedList<WarehouseItemListDto>.CreateAsync(projectedList, pageIndex, pageSize);
            }
            catch (Exception e)
            {
                string msg=String.Empty;
                if (e.InnerException!=null)
                {
                    msg = e.InnerException.Message;
                }
                
                return BadRequest(new
                {
                    error = e.Message + " " + msg
                });

            }
            foreach (var productItem in listItems)
            {

                ProductMedia productMedia;

                try
                {
                    productMedia = await _context.ProductMedia
                        .Include(p => p.MediaEntry)
                        .SingleOrDefaultAsync(p => p.ProductId == productItem.Id);
                    productItem.Url = productMedia != null ? Url.Content("~/productimages/" + productMedia.MediaEntry.MediaFile) : Url.Content("~/productimages/" + "noimage.jpg");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    productItem.Url = Url.Content("~/productimages/" + "noimage.jpg");
                }

            }

            var response = new IndexDataTableResponse<WarehouseItemListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                // Diaries = relevantDiarys,
                Data = listItems
            };
            return Ok(response);
        }

        [HttpGet("GetIndexTblDataWareHouseSelectorItems")]
        public async Task<IActionResult> GetIndexTblDataWareHouseSelectorItems([FromQuery] IndexDataTableRequest request)
        {
            //Thread.Sleep(10000);
            IQueryable<WarehouseItem> fullListIq = _context.WarehouseItems;
            if (!String.IsNullOrEmpty(request.WarehouseItemNatureFilter))
            {
                if (Int32.TryParse(request.WarehouseItemNatureFilter, out var warehouseItemNatureFilter))
                {
                    if (warehouseItemNatureFilter > 0)
                    {
                        var flt = (WarehouseItemNatureEnum)warehouseItemNatureFilter;
                        fullListIq = fullListIq.Where(p => p.WarehouseItemNature == flt);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {

                    case "namesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Name);
                        break;
                    case "namesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Name);
                        break;
                    case "codesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Code);
                        break;
                    case "codesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Code);
                        break;
                    case "categorysort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.MaterialCaterory.Name);
                        break;
                    case "categorysort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.MaterialCaterory.Name);
                        break;
                }
            }

            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        var allCompCode =
                            await _context.AppSettings.SingleOrDefaultAsync(
                                p => p.Code == Constants.AllCompaniesCodeKey);
                        if (allCompCode == null)
                        {
                            return NotFound("All Companies Code Setting not found");
                        }
                        var allCompaniesEntity = await _context.Companies.SingleOrDefaultAsync(s => s.Code == allCompCode.Value);
                        if (allCompaniesEntity != null)
                        {
                            var allCompaniesId = allCompaniesEntity.Id;
                            fullListIq = fullListIq.Where(p => p.CompanyId == companyId || p.CompanyId == allCompaniesId);
                        }
                        else
                        {
                            fullListIq = fullListIq.Where(p => p.CompanyId == companyId);
                        }


                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.Name.Contains(request.SearchFilter)
                                                   || p.Code.Contains(request.SearchFilter)
                                                   || p.ShortDescription.Contains(request.SearchFilter)
                                                   || p.Description.Contains(request.SearchFilter)
                                                   || p.MaterialCaterory.Name.Contains(request.SearchFilter));
            }

            PagedList<WarehouseItemListDto> listItems;
            try
            {
                var projectedList = fullListIq.ProjectTo<WarehouseItemListDto>(_mapper.ConfigurationProvider);
                var pageIndex = request.PageIndex;

                var pageSize = request.PageSize;

                listItems = await PagedList<WarehouseItemListDto>.CreateAsync(projectedList, pageIndex, pageSize);
            }
            catch (Exception e)
            {
                string msg = e.InnerException.Message;
                return BadRequest(new
                {
                    error = e.Message + " " + msg
                });

            }
            foreach (var productItem in listItems)
            {

                ProductMedia productMedia;

                try
                {
                    productMedia = await _context.ProductMedia
                        .Include(p => p.MediaEntry)
                        .SingleOrDefaultAsync(p => p.ProductId == productItem.Id);
                    if (productMedia != null)
                    {
                        productItem.Url = Url.Content("~/productimages/" + productMedia.MediaEntry.MediaFile);
                    }
                    else
                    {
                        productItem.Url = Url.Content("~/productimages/" + "noimage.jpg");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    productItem.Url = Url.Content("~/productimages/" + "noimage.jpg");
                }

            }

            var response = new IndexDataTableResponse<WarehouseItemListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                // Diaries = relevantDiarys,
                Data = listItems
            };
            return Ok(response);
        }
        private string GetDocSeriesName(RecurringDocTypeEnum docType, int docSeriesId, IList<BuyDocSeriesDef> buyDocSeriesList, IList<SellDocSeriesDef> sellDocSeriesList)
        {
            string docName=string.Empty;
            string docCode=string.Empty;

            switch (docType)
            {
                case RecurringDocTypeEnum.BuyType:
                    var docBuySeries = buyDocSeriesList.FirstOrDefault(p => p.Id == docSeriesId);
                    if (docBuySeries != null)
                    {
                        docName = docBuySeries.Name;
                        docCode = docBuySeries.Code;
                    }
                    break;
                case RecurringDocTypeEnum.SellType:
                    var docSellSeries = sellDocSeriesList.FirstOrDefault(p => p.Id == docSeriesId);
                    if (docSellSeries != null)
                    {
                        docName = docSellSeries.Name;
                        docCode = docSellSeries.Code;
                    }
                    break;
                default:
                    break;
            }

            return docName;
        }
        private string GetRecurringTypeName(RecurringDocTypeEnum recurringType)
        {
            string typeName = string.Empty;
            switch (recurringType)
            {
                case RecurringDocTypeEnum.BuyType:
                    typeName = "Αγορές/Εξοδα";
                    break;
                case RecurringDocTypeEnum.SellType:
                    typeName = "Πωλήσεις/Εσοδα";
                    break;
                default:
                    typeName = "#Απροσδιόριστο#";
                    break;
            }

            return typeName;
        }
        private string GetDocSeriesCode(RecurringDocTypeEnum docType, int docSeriesId, IList<BuyDocSeriesDef> buyDocSeriesList,IList<SellDocSeriesDef> sellDocSeriesList)
        {
            string docName = string.Empty;
            string docCode = string.Empty;

            switch (docType)
            {
                case RecurringDocTypeEnum.BuyType:
                    var docBuySeries = buyDocSeriesList.FirstOrDefault(p => p.Id == docSeriesId);
                    if (docBuySeries != null)
                    {
                        docName = docBuySeries.Name;
                        docCode = docBuySeries.Code;
                    }
                    break;
                case RecurringDocTypeEnum.SellType:
                    var docSellSeries = sellDocSeriesList.FirstOrDefault(p => p.Id == docSeriesId);
                    if (docSellSeries != null)
                    {
                        docName = docSellSeries.Name;
                        docCode = docSellSeries.Code;
                    }
                    break;
                default:
                    break;
            }

            return docCode;
        }
        private decimal ConvertAmount(int companyCurrencyId,int displayCurrencyId,IList<ExchangeRate> rates,decimal amount)
        {
            //var r =  rates.Where(p => p.CurrencyId == companyCurrencyId)
            //    .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
            decimal retAmount=amount;
            if (displayCurrencyId==companyCurrencyId)
            {
                return retAmount;
            }
            if (companyCurrencyId != 1)
            {
                var r = rates.Where(p => p.CurrencyId == companyCurrencyId)
                    .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                if (r != null)
                {
                    retAmount = amount / r.Rate;
                }
            }
            if (displayCurrencyId != 1)
            {
                var r = rates.Where(p => p.CurrencyId == displayCurrencyId)
                    .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                if (r != null)
                {
                    retAmount *= r.Rate;

                }
            }
            return retAmount;
        }
        [HttpGet("LastDiaryTransactionData")]
        public async Task<IActionResult> GetLastDiaryTransactionDataAsync(int transactorId)
        {
            var trDto = await _context.FinDiaryTransactions
                .OrderByDescending(p => p.TransactionDate)
                .FirstOrDefaultAsync(p => p.TransactorId == transactorId);

            if (trDto != null)
            {
                var cat = new LastDiaryTransactionsData
                {
                    CategoryId = trDto.FinTransCategoryId,
                    CostCentreId = trDto.CostCentreId,
                    RevenueCentreId = trDto.RevenueCentreId,
                    CompanyId = trDto.CompanyId
                };
                return Ok(cat);
            }

            return NotFound();
        }
        [HttpGet("GetIndexTblDataTransactorDiary")]
        public async Task<IActionResult> GetIndexTblDataTransactorDiary([FromQuery] IndexDataTableRequest request)
        {
            if (request.DiaryId <= 0)
            {
                return BadRequest(new
                {
                    Error = "No valid diary id specified"
                });
            }
            var diaryDef = await _context.DiaryDefs.FindAsync(request.DiaryId);
            if (diaryDef == null)
            {
                return NotFound(new
                {
                    Error = "Diary Id not found"
                });
            }

            IQueryable<TransactorTransaction> transactionsList = _context.TransactorTransactions;

            if (diaryDef.SelectedTransTypes != null)
            {
                var transTypes = Array.ConvertAll(diaryDef.SelectedTransTypes.Split(","), int.Parse);
                transactionsList = transactionsList.Where(p => transTypes.Contains(p.Transactor.TransactorTypeId));
            }
            if (diaryDef.SelectedDocTypes != null)
            {
                var docTypes = Array.ConvertAll(diaryDef.SelectedDocTypes.Split(","), int.Parse);
                transactionsList = transactionsList.Where(p => docTypes.Contains(p.TransTransactorDocTypeId));
            }

            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "transactiondatesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.TransDate);
                        break;
                    case "transactiondatesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.TransDate);
                        break;
                    case "transactornamesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.Transactor.Name);
                        break;
                    case "transactornamesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.Transactor.Name);
                        break;
                    case "seriescodesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.TransTransactorDocSeries.Code);
                        break;
                    case "seriescodesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.TransTransactorDocSeries.Name);
                        break;
                    case "companycodesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.Company.Code);
                        break;
                    case "companycodesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.Company.Name);
                        break;
                }
            }
            if (!String.IsNullOrEmpty(request.DateRange))
            {
                var datePeriodFilter = request.DateRange;
                DateFilterDates dfDates = DateFilter.GetDateFilterDates(datePeriodFilter);
                DateTime fromDate = dfDates.FromDate;
                DateTime toDate = dfDates.ToDate;

                transactionsList = transactionsList.Where(p => p.TransDate >= fromDate && p.TransDate <= toDate);
            }
            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        transactionsList = transactionsList.Where(p => p.CompanyId == companyId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                transactionsList = transactionsList.Where(p => p.Transactor.Name.Contains(request.SearchFilter));
            }

            var currencyRates = await _context.ExchangeRates.OrderByDescending(p => p.ClosingDate)
                .Take(10)
                .ToListAsync();
            var t = transactionsList.ProjectTo<TransactorTransListDto>(_mapper.ConfigurationProvider);
            var t1 =await  t.Select(p => new TransactorTransListDto
            {
                //Id = p.Id,
                //TransDate = p.TransDate,
                TransTransactorDocSeriesId = p.TransTransactorDocSeriesId,
                //TransTransactorDocSeriesName = p.TransTransactorDocSeriesName,
                TransTransactorDocSeriesCode = p.TransTransactorDocSeriesCode,
                TransTransactorDocTypeId = p.TransTransactorDocTypeId,
                //TransRefCode = p.TransRefCode,
                //TransactorId = p.TransactorId,
                //TransactorName = p.TransactorName,
                SectionId = p.SectionId,
                //SectionCode = p.SectionCode,
                //CreatorId = p.CreatorId,
                FiscalPeriodId = p.FiscalPeriodId,
                FinancialAction = p.FinancialAction,
                FpaRate = p.FpaRate,
                DiscountRate = p.DiscountRate,
                AmountFpa =    ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountFpa),
                AmountNet = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountNet),
                
                AmountDiscount = ConvertAmount(p.CompanyCurrencyId,request.DisplayCurrencyId,currencyRates, p.AmountDiscount),
                TransFpaAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.TransFpaAmount),
                TransNetAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.TransNetAmount),
                TransDiscountAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.TransDiscountAmount),
                CompanyCode = p.CompanyCode,
                CompanyCurrencyId = p.CompanyCurrencyId
            }).ToListAsync();
            var gransSumOfAmount =  t1.Sum(p => p.TotalAmount);
            var gransSumOfDebit =  t1.Sum(p => p.DebitAmount);
            var gransSumOfCredit =  t1.Sum(p => p.CreditAmount);
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<TransactorTransListDto>.CreateAsync(t, pageIndex, pageSize);
            foreach (var listItem in listItems)
            {
                if (listItem.CompanyCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == listItem.CompanyCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        listItem.AmountFpa = listItem.AmountFpa / r.Rate;
                        listItem.AmountNet = listItem.AmountNet / r.Rate;
                        listItem.AmountDiscount = listItem.AmountDiscount / r.Rate;
                        listItem.TransFpaAmount = listItem.TransFpaAmount / r.Rate;
                        listItem.TransNetAmount = listItem.TransNetAmount / r.Rate;
                        listItem.TransDiscountAmount = listItem.TransDiscountAmount / r.Rate;

                    }
                }
                if (request.DisplayCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == request.DisplayCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        listItem.AmountFpa = listItem.AmountFpa * r.Rate;
                        listItem.AmountNet = listItem.AmountNet * r.Rate;
                        listItem.AmountDiscount = listItem.AmountDiscount * r.Rate;
                        listItem.TransFpaAmount = listItem.TransFpaAmount * r.Rate;
                        listItem.TransNetAmount = listItem.TransNetAmount * r.Rate;
                        listItem.TransDiscountAmount = listItem.TransDiscountAmount * r.Rate;

                    }
                }

            }
            decimal sumAmountTotal = listItems.Sum(p => p.TotalAmount);
            decimal sumDebit = listItems.Sum(p => p.DebitAmount);
            decimal sumCredit = listItems.Sum(p => p.CreditAmount);
            var response = new IndexDataTableResponse<TransactorTransListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                SumOfAmount = sumAmountTotal,
                SumOfDebit = sumDebit,
                SumOfCredit = sumCredit,
                GrandSumOfAmount = gransSumOfAmount,
                GrandSumOfDebit = gransSumOfDebit,
                GrandSumOfCredit = gransSumOfCredit,
                Data = listItems
            };

            return Ok(response);
        }
        [HttpGet("GetIndexTblDataTransactorAccountTab")]
        public async Task<IActionResult> GetIndexTblDataTransactorAccountTab([FromQuery] IndexDataTableRequest request)
        {
            if (request.TransactorId <= 0)
            {
                return BadRequest(new
                {
                    Error = "No valid transactor id specified"
                });
            }

            var transactor = await _context.Transactors.FirstOrDefaultAsync(x => x.Id == request.TransactorId);
            if (transactor == null)
            {
                return NotFound(new
                {
                    Error = "Transactor not found"
                });
            }
            var transactorType = await _context.TransactorTypes.Where(c => c.Id == transactor.TransactorTypeId).FirstOrDefaultAsync();

            IQueryable<TransactorTransaction> transactionsList = _context.TransactorTransactions
                .Where(p => p.TransactorId == request.TransactorId);
            IQueryable<TransactorTransaction> transListBeforePeriod = _context.TransactorTransactions
                .Where(p => p.TransactorId == request.TransactorId);
            IQueryable<TransactorTransaction> transListAll = _context.TransactorTransactions
                .Where(p => p.TransactorId == request.TransactorId);
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "datesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.TransDate);
                        break;
                    case "datesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.TransDate);
                        break;
                    case "transactornamesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.Transactor.Name);
                        break;
                    case "transactornamesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.Transactor.Name);
                        break;
                    case "seriescodesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.TransTransactorDocSeries.Code);
                        break;
                    case "seriescodesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.TransTransactorDocSeries.Code);
                        break;
                    case "companycodesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.Company.Code);
                        break;
                    case "companycodesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.Company.Code);
                        break;
                }
            }

            DateTime beforePeriodDate = DateTime.Today;
            if (!String.IsNullOrEmpty(request.DateRange))
            {
                var datePeriodFilter = request.DateRange;
                DateFilterDates dfDates = DateFilter.GetDateFilterDates(datePeriodFilter);
                DateTime fromDate = dfDates.FromDate;
                beforePeriodDate = fromDate.AddDays(-1);
                DateTime toDate = dfDates.ToDate;

                transactionsList = transactionsList.Where(p => p.TransDate >= fromDate && p.TransDate <= toDate);
                transListBeforePeriod = transListBeforePeriod.Where(p => p.TransDate < fromDate);

            }
            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        transactionsList = transactionsList.Where(p => p.CompanyId == companyId);
                        transListBeforePeriod = transListBeforePeriod.Where(p => p.CompanyId == companyId);
                        transListAll = transListAll.Where(p => p.CompanyId == companyId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                transactionsList = transactionsList.Where(p => p.TransTransactorDocSeries.Name.Contains(request.SearchFilter)
                || p.TransTransactorDocSeries.Code.Contains(request.SearchFilter)
                || p.TransRefCode.Contains(request.SearchFilter)
                );
                transListBeforePeriod = transListBeforePeriod.Where(p => p.TransTransactorDocSeries.Name.Contains(request.SearchFilter)
                                                                         || p.TransTransactorDocSeries.Code.Contains(request.SearchFilter)
                                                                         || p.TransRefCode.Contains(request.SearchFilter)

                );
                transListAll = transListAll.Where(p => p.TransTransactorDocSeries.Name.Contains(request.SearchFilter)
                                                       || p.TransTransactorDocSeries.Code.Contains(request.SearchFilter)
                                                       || p.TransRefCode.Contains(request.SearchFilter)

                );
            }
            var dbTrans = transactionsList.ProjectTo<TransactorTransListDto>(_mapper.ConfigurationProvider);
           
            var currencyRates = await _context.ExchangeRates.OrderByDescending(p => p.ClosingDate)
                .Take(10)
                .ToListAsync();
            var t = transListAll.ProjectTo<TransactorTransListDto>(_mapper.ConfigurationProvider);
            var t1 = await t.Select(p => new TransactorTransListDto
            {
                Id = p.Id,
                TransDate = p.TransDate,
                TransTransactorDocSeriesId = p.TransTransactorDocSeriesId,
                TransTransactorDocSeriesName = p.TransTransactorDocSeriesName,
                TransTransactorDocSeriesCode = p.TransTransactorDocSeriesCode,
                TransTransactorDocTypeId = p.TransTransactorDocTypeId,
                TransRefCode = p.TransRefCode,
                TransactorId = p.TransactorId,
                TransactorName = p.TransactorName,
                SectionId = p.SectionId,
                SectionCode = p.SectionCode,
                CreatorId = p.CreatorId,
                FiscalPeriodId = p.FiscalPeriodId,
                FinancialAction = p.FinancialAction,
                FpaRate = p.FpaRate,
                DiscountRate = p.DiscountRate,
                AmountFpa = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountFpa),
                AmountNet = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountNet),
                AmountDiscount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates,
                   p.AmountDiscount),
                TransFpaAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.TransFpaAmount),
                TransNetAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.TransNetAmount),
                TransDiscountAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.TransDiscountAmount),
                CompanyCode = p.CompanyCode,
                CompanyCurrencyId = p.CompanyCurrencyId
            }).ToListAsync();
            var grandSumOfAmount = t1.Sum(p => p.TotalAmount);
            var grandSumOfDebit = t1.Sum(p => p.DebitAmount);
            var grandSumOfCredit = t1.Sum(p => p.CreditAmount);
            var dbTransactions = await dbTrans.ToListAsync();
            foreach (var listItem in dbTransactions)
            {
                if (listItem.CompanyCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == listItem.CompanyCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        listItem.AmountFpa = listItem.AmountFpa / r.Rate;
                        listItem.AmountNet = listItem.AmountNet / r.Rate;
                        listItem.AmountDiscount = listItem.AmountDiscount / r.Rate;
                        listItem.TransFpaAmount = listItem.TransFpaAmount / r.Rate;
                        listItem.TransNetAmount = listItem.TransNetAmount / r.Rate;
                        listItem.TransDiscountAmount = listItem.TransDiscountAmount / r.Rate;
                    }
                }
                if (request.DisplayCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == request.DisplayCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        listItem.AmountFpa = listItem.AmountFpa * r.Rate;
                        listItem.AmountNet = listItem.AmountNet * r.Rate;
                        listItem.AmountDiscount = listItem.AmountDiscount * r.Rate;
                        listItem.TransFpaAmount = listItem.TransFpaAmount * r.Rate;
                        listItem.TransNetAmount = listItem.TransNetAmount * r.Rate;
                        listItem.TransDiscountAmount = listItem.TransDiscountAmount * r.Rate;
                    }
                }

            }
            //-----------------------------------------------
            var dbTransBeforePeriod = transListBeforePeriod.ProjectTo<TransactorTransListDto>(_mapper.ConfigurationProvider);
            var transBeforePeriodList = await dbTransBeforePeriod.ToListAsync();
            foreach (var item in transBeforePeriodList)
            {
                if (item.CompanyCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == item.CompanyCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        item.AmountFpa = item.AmountFpa / r.Rate;
                        item.AmountNet = item.AmountNet / r.Rate;
                        item.AmountDiscount = item.AmountDiscount / r.Rate;
                        item.TransFpaAmount = item.TransFpaAmount / r.Rate;
                        item.TransNetAmount = item.TransNetAmount / r.Rate;
                        item.TransDiscountAmount = item.TransDiscountAmount / r.Rate;
                    }
                }
                if (request.DisplayCurrencyId != 1)
                {
                    var r = currencyRates.Where(p => p.CurrencyId == request.DisplayCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefault();
                    if (r != null)
                    {
                        item.AmountFpa = item.AmountFpa * r.Rate;
                        item.AmountNet = item.AmountNet * r.Rate;
                        item.AmountDiscount = item.AmountDiscount * r.Rate;
                        item.TransFpaAmount = item.TransFpaAmount * r.Rate;
                        item.TransNetAmount = item.TransNetAmount * r.Rate;
                        item.TransDiscountAmount = item.TransDiscountAmount * r.Rate;
                    }
                }

            }
            //Create before period line
            var bl1 = new
            {

                Debit = transBeforePeriodList.Sum(x => x.DebitAmount),
                Credit = transBeforePeriodList.Sum(x => x.CreditAmount),
            };
           
            var beforePeriod = new KartelaLine();

            beforePeriod.Credit = bl1.Credit;
            beforePeriod.Debit = bl1.Debit;
            switch (transactorType.Code)
            {
                case "SYS.DTRANSACTOR":

                    break;
                case "SYS.CUSTOMER":
                    beforePeriod.RunningTotal = bl1.Debit - bl1.Credit;
                    break;
                case "SYS.SUPPLIER":
                    beforePeriod.RunningTotal = bl1.Credit - bl1.Debit;
                    break;
                default:
                    beforePeriod.RunningTotal = bl1.Credit - bl1.Debit;
                    break;
            }
            beforePeriod.TransDate = beforePeriodDate;
            beforePeriod.DocSeriesCode = "Εκ.Μεταφ.";
            beforePeriod.CreatorId = -1;
            beforePeriod.TransactorName = "";

            var listWithTotal = new List<KartelaLine>();
            listWithTotal.Add(beforePeriod);

            //----------------------------------------------------


            decimal runningTotal = beforePeriod.RunningTotal;
            foreach (var dbTransaction in dbTransactions)
            {
                switch (transactorType.Code)
                {
                    case "SYS.DTRANSACTOR":

                        break;
                    case "SYS.CUSTOMER":
                        runningTotal = dbTransaction.DebitAmount - dbTransaction.CreditAmount + runningTotal;
                        break;
                    case "SYS.SUPPLIER":
                        runningTotal = dbTransaction.CreditAmount - dbTransaction.DebitAmount + runningTotal;
                        break;
                    default:
                        runningTotal = dbTransaction.CreditAmount - dbTransaction.DebitAmount + runningTotal;
                        break;
                }


                listWithTotal.Add(new KartelaLine
                {
                    TransDate = dbTransaction.TransDate,
                    DocSeriesCode = dbTransaction.TransTransactorDocSeriesCode,
                    RefCode = dbTransaction.TransRefCode,
                    CompanyCode = dbTransaction.CompanyCode,
                    SectionCode = dbTransaction.SectionCode,
                    CreatorId = dbTransaction.SectionCode== "SCNTRANSACTORTRANS" ? dbTransaction.Id:dbTransaction.CreatorId,
                    RunningTotal = runningTotal,
                    TransactorName = dbTransaction.TransactorName,
                    Debit = dbTransaction.DebitAmount,
                    Credit = dbTransaction.CreditAmount
                });
            }

            var outList = listWithTotal.AsQueryable();
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;
            decimal sumCredit = 0;
            decimal sumDebit = 0;
            decimal sumDifference = 0;

            IQueryable<KartelaLine> fullListIq = from s in outList select s;

            var listItems = PagedList<KartelaLine>.Create(fullListIq, pageIndex, pageSize);

            foreach (var item in listItems)
            {
                sumCredit += item.Credit;
                sumDebit += item.Debit;
            }
            switch (transactorType.Code)
            {
                case "SYS.DTRANSACTOR":

                    break;
                case "SYS.CUSTOMER":
                    sumDifference = sumDebit - sumCredit;
                    break;
                case "SYS.SUPPLIER":
                    sumDifference = sumCredit - sumDebit;
                    break;
                default:
                    sumDifference = sumCredit - sumDebit;
                    break;
            }

            var response = new IndexDataTableResponse<KartelaLine>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                SumOfDebit = sumDebit,
                SumOfCredit = sumCredit,
                SumOfDifference = sumDifference,
                GrandSumOfAmount = grandSumOfAmount,
                GrandSumOfDebit = grandSumOfDebit,
                GrandSumOfCredit = grandSumOfCredit,
                Data = listItems
            };
            return Ok(response);
        }
        [HttpGet("GetIndexTblDataWarehouseAccountTab")]
        public async Task<IActionResult> GetIndexTblDataWarehouseAccountTab([FromQuery] IndexDataTableRequest request)
        {
            if (request.WarehouseItemId <= 0)
            {
                return BadRequest(new
                {
                    Error = "No valid warehouse item id specified"
                });
            }

            var warehouseItem = await _context.WarehouseItems.FirstOrDefaultAsync(x => x.Id == request.WarehouseItemId);
            if (warehouseItem == null)
            {
                return NotFound(new
                {
                    Error = "warehouse Item not found"
                });
            }
            // var transactorType = await _context.TransactorTypes.Where(c => c.Id == warehouseItem.TransactorTypeId).FirstOrDefaultAsync();

            IQueryable<WarehouseTransaction> transactionsList = _context.WarehouseTransactions
                .Where(p => p.WarehouseItemId == request.WarehouseItemId);
            IQueryable<WarehouseTransaction> transListBeforePeriod = _context.WarehouseTransactions
                .Where(p => p.WarehouseItemId == request.WarehouseItemId);
            IQueryable<WarehouseTransaction> transListAll = _context.WarehouseTransactions
                .Where(p => p.WarehouseItemId == request.WarehouseItemId);
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "datesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.TransDate);
                        break;
                    case "datesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.TransDate);
                        break;
                   
                    case "seriescodesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.TransWarehouseDocSeries.Code);
                        break;
                    case "seriescodesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.TransWarehouseDocSeries.Code);
                        break;
                    case "companycodesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.Company.Code);
                        break;
                    case "companycodesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.Company.Code);
                        break;
                }
            }

            DateTime beforePeriodDate = DateTime.Today;
            if (!String.IsNullOrEmpty(request.DateRange))
            {
                var datePeriodFilter = request.DateRange;
                DateFilterDates dfDates = DateFilter.GetDateFilterDates(datePeriodFilter);
                DateTime fromDate = dfDates.FromDate;
                beforePeriodDate = fromDate.AddDays(-1);
                DateTime toDate = dfDates.ToDate;

                transactionsList = transactionsList.Where(p => p.TransDate >= fromDate && p.TransDate <= toDate);
                transListBeforePeriod = transListBeforePeriod.Where(p => p.TransDate < fromDate);

            }
            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        transactionsList = transactionsList.Where(p => p.CompanyId == companyId);
                        transListBeforePeriod = transListBeforePeriod.Where(p => p.CompanyId == companyId);
                        transListAll = transListAll.Where(p => p.CompanyId == companyId);
                    }
                }
            }
            var currencyRates = await _context.ExchangeRates.OrderByDescending(p => p.ClosingDate)
                .Take(10)
                .ToListAsync();
            var dbTrans = transactionsList.ProjectTo<WarehouseTransListDto>(_mapper.ConfigurationProvider);
            var t = transListAll.ProjectTo<WarehouseTransListDto>(_mapper.ConfigurationProvider);
            var t1 =await t.Select(p => new WarehouseTransListDto
            {
                Id = p.Id,
                TransDate = p.TransDate,
                TransWarehouseDocSeriesId = p.TransWarehouseDocSeriesId,
                TransWarehouseDocSeriesName = p.TransWarehouseDocSeriesName,
                TransWarehouseDocSeriesCode = p.TransWarehouseDocSeriesCode,
                TransRefCode = p.TransRefCode,
                SectionCode = p.SectionCode,
                CreatorId = p.CreatorId,
                WarehouseItemId = p.WarehouseItemId,
                WarehouseItemName = p.WarehouseItemName,
                TransactionType = p.TransactionType,
                InventoryAction = p.InventoryAction,
                InventoryValueAction = p.InventoryValueAction,
                InvoicedVolumeAction = p.InvoicedVolumeAction,
                InvoicedValueAction = p.InvoicedValueAction,
                Quontity1 = p.Quontity1,
                Quontity2 = p.Quontity2,
                UnitPrice = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.UnitPrice),
                UnitExpenses = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.UnitExpenses),
                UnitPriceFinal = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.UnitPriceFinal),
                AmountFpa = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountFpa),
                AmountNet = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountNet),
                AmountDiscount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.AmountDiscount),
                TransQ1 = p.TransQ1,
                TransQ2 = p.TransQ2,
                TransFpaAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.TransFpaAmount),
                TransNetAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.TransNetAmount),
                TransDiscountAmount = ConvertAmount(p.CompanyCurrencyId, request.DisplayCurrencyId, currencyRates, p.TransDiscountAmount),
                CompanyId = p.CompanyId,
                CompanyCode = p.CompanyCode,
                CompanyCurrencyId = p.CompanyCurrencyId
            }).ToListAsync();
            decimal grandSumImportVolume = t1.Sum(p => p.ImportUnits);
            decimal grandSumImportValue = t1.Sum(p => p.ImportAmount);
            decimal grandSumExportVolume = t1.Sum(p => p.ExportUnits);
            decimal grandSumExportValue = t1.Sum(p => p.ExportAmount);
            var dbTransactions = await dbTrans.ToListAsync();
            foreach (var listItem in dbTransactions)
            {
                if (listItem.CompanyCurrencyId != 1)
                {
                    var r = await _context.ExchangeRates.Where(p => p.CurrencyId == listItem.CompanyCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefaultAsync();
                    if (r != null)
                    {
                        listItem.AmountFpa = listItem.AmountFpa / r.Rate;
                        listItem.AmountNet = listItem.AmountNet / r.Rate;
                        listItem.AmountDiscount = listItem.AmountDiscount / r.Rate;
                        listItem.TransFpaAmount = listItem.TransFpaAmount / r.Rate;
                        listItem.TransNetAmount = listItem.TransNetAmount / r.Rate;
                        listItem.TransDiscountAmount = listItem.TransDiscountAmount / r.Rate;
                    }
                }
                if (request.DisplayCurrencyId != 1)
                {
                    var r = await _context.ExchangeRates.Where(p => p.CurrencyId == request.DisplayCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefaultAsync();
                    if (r != null)
                    {
                        listItem.AmountFpa = listItem.AmountFpa * r.Rate;
                        listItem.AmountNet = listItem.AmountNet * r.Rate;
                        listItem.AmountDiscount = listItem.AmountDiscount * r.Rate;
                        listItem.TransFpaAmount = listItem.TransFpaAmount * r.Rate;
                        listItem.TransNetAmount = listItem.TransNetAmount * r.Rate;
                        listItem.TransDiscountAmount = listItem.TransDiscountAmount * r.Rate;
                    }
                }

            }
            var dbTransBeforePeriod = transListBeforePeriod.ProjectTo<WarehouseTransListDto>(_mapper.ConfigurationProvider);
            var transBeforePeriodList = await dbTransBeforePeriod.ToListAsync();
            foreach (var listItem in transBeforePeriodList)
            {
                if (listItem.CompanyCurrencyId != 1)
                {
                    var r = await _context.ExchangeRates.Where(p => p.CurrencyId == listItem.CompanyCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefaultAsync();
                    if (r != null)
                    {
                        listItem.AmountFpa = listItem.AmountFpa / r.Rate;
                        listItem.AmountNet = listItem.AmountNet / r.Rate;
                        listItem.AmountDiscount = listItem.AmountDiscount / r.Rate;
                        listItem.TransFpaAmount = listItem.TransFpaAmount / r.Rate;
                        listItem.TransNetAmount = listItem.TransNetAmount / r.Rate;
                        listItem.TransDiscountAmount = listItem.TransDiscountAmount / r.Rate;
                    }
                }
                if (request.DisplayCurrencyId != 1)
                {
                    var r = await _context.ExchangeRates.Where(p => p.CurrencyId == request.DisplayCurrencyId)
                        .OrderByDescending(p => p.ClosingDate).FirstOrDefaultAsync();
                    if (r != null)
                    {
                        listItem.AmountFpa = listItem.AmountFpa * r.Rate;
                        listItem.AmountNet = listItem.AmountNet * r.Rate;
                        listItem.AmountDiscount = listItem.AmountDiscount * r.Rate;
                        listItem.TransFpaAmount = listItem.TransFpaAmount * r.Rate;
                        listItem.TransNetAmount = listItem.TransNetAmount * r.Rate;
                        listItem.TransDiscountAmount = listItem.TransDiscountAmount * r.Rate;
                    }
                }

            }
            //Create before period line
            var bl1 = new
            {
                ImportVolume = transBeforePeriodList.Sum(x => x.ImportUnits),
                ExportVolume = transBeforePeriodList.Sum(x => x.ExportUnits),

                ImportValue = transBeforePeriodList.Sum(x => x.ImportAmount),
                ExportValue = transBeforePeriodList.Sum(x => x.ExportAmount)
            };
            var beforePeriod = new WarehouseKartelaLine();

            if (Math.Abs(bl1.ImportVolume) > Math.Abs(bl1.ExportVolume))
            {
                beforePeriod.ImportVolume = bl1.ImportVolume - bl1.ExportVolume;
                beforePeriod.ExportVolume = 0;
            }
            else
            {
                beforePeriod.ImportVolume = 0;
                beforePeriod.ExportVolume = bl1.ExportVolume - bl1.ImportVolume;
            }
            if (Math.Abs(bl1.ImportValue) > Math.Abs(bl1.ExportValue))
            {
                beforePeriod.ImportValue = bl1.ImportValue - bl1.ExportValue;
                beforePeriod.ExportValue = 0;
            }
            else
            {
                beforePeriod.ImportValue = 0;
                beforePeriod.ExportValue = bl1.ExportValue - bl1.ImportValue;
            }
            beforePeriod.RunningTotalVolume = bl1.ImportVolume - bl1.ExportVolume;
            beforePeriod.RunningTotalValue = bl1.ImportValue - bl1.ExportValue;
            beforePeriod.TransDate = beforePeriodDate;
            beforePeriod.DocSeriesCode = "Εκ.Μεταφ.";
            beforePeriod.CreatorId = -1;
            beforePeriod.MaterialName = "";

            var listWithTotal = new List<WarehouseKartelaLine>();
            listWithTotal.Add(beforePeriod);
            // decimal runningTotal = 0;
            decimal runningTotalVolume = beforePeriod.RunningTotalVolume;
            decimal runningTotalValue = beforePeriod.RunningTotalValue;
            foreach (var dbTransaction in dbTransactions)
            {
                runningTotalVolume = dbTransaction.ImportUnits - dbTransaction.ExportUnits + runningTotalVolume;
                runningTotalValue = dbTransaction.ImportAmount - dbTransaction.ExportAmount + runningTotalValue;
                listWithTotal.Add(new WarehouseKartelaLine
                {
                    TransDate = dbTransaction.TransDate,
                    DocSeriesCode = dbTransaction.TransWarehouseDocSeriesCode,
                    RefCode = dbTransaction.TransRefCode,
                    CompanyCode = dbTransaction.CompanyCode,
                    SectionCode = dbTransaction.SectionCode,
                    CreatorId = dbTransaction.SectionCode == "SCNWARHSETRANS" ? dbTransaction.Id : dbTransaction.CreatorId,
                    RunningTotalVolume = runningTotalVolume,
                    RunningTotalValue = runningTotalValue,
                    MaterialName = dbTransaction.WarehouseItemName,
                    ImportVolume = dbTransaction.ImportUnits,
                    ExportVolume = dbTransaction.ExportUnits,
                    ImportValue = dbTransaction.ImportAmount,
                    ExportValue = dbTransaction.ExportAmount
                });
            }


            var outList = listWithTotal.AsQueryable();
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;
            decimal sumImportsVolume = 0;
            decimal sumExportsVolume = 0;
            decimal sumImportsValue = 0;
            decimal sumExportsValue = 0;
            //  decimal sumDifference = 0;

            IQueryable<WarehouseKartelaLine> fullListIq = from s in outList select s;
           
            var listItems = PagedList<WarehouseKartelaLine>.Create(fullListIq, pageIndex, pageSize);

            foreach (var item in listItems)
            {
                sumImportsVolume += item.ImportVolume;
                sumExportsVolume += item.ExportVolume;
                sumImportsValue += item.ImportValue;
                sumExportsValue += item.ExportValue;
            }


            var response = new IndexDataTableResponse<WarehouseKartelaLine>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                SumImportValue = sumImportsValue,
                SumExportValue = sumExportsValue,
                SumImportVolume = sumImportsVolume,
                SumExportVolume = sumExportsVolume,
                GrandSumImportVolume = grandSumImportVolume,
                GrandSumImportValue = grandSumImportValue,
                GrandSumExportVolume = grandSumExportVolume,
                GrandSumExportValue = grandSumExportValue,
                Data = listItems
            };
            return Ok(response);
        }
        [HttpGet("GetIndexTblDataCashRegCategoryProductItems")]
        public async Task<IActionResult> GetIndexTblDataCashRegCategoryProductItems([FromQuery] IndexDataTableRequest request)
        {
            IQueryable<CrCatWarehouseItem> fullListIq = _context.CrCatWarehouseItems;

            if (!String.IsNullOrEmpty(request.CashRegCategoryFilter))
            {
                if (Int32.TryParse(request.CashRegCategoryFilter, out var cashRegCategoryId))
                {
                    if (cashRegCategoryId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.CashRegCategoryId == cashRegCategoryId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.ClientProfileFilter))
            {
                if (Int32.TryParse(request.ClientProfileFilter, out var clientProfileId))
                {
                    if (clientProfileId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.ClientProfileId == clientProfileId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                if (Int32.TryParse(request.CompanyFilter, out var companyId))
                {
                    if (companyId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.ClientProfile.CompanyId == companyId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {

                    case "namesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.WarehouseItem.Name);
                        break;
                    case "namesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.WarehouseItem.Name);
                        break;
                    case "categorysort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.CashRegCategory.Name);
                        break;
                    case "categorysort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.CashRegCategory.Name);
                        break;
                }
            }


            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.WarehouseItem.Name.Contains(request.SearchFilter)
                                                   || p.ClientProfile.Name.Contains(request.SearchFilter)
                                                   || p.CashRegCategory.Name.Contains(request.SearchFilter)
                                                   || p.ClientProfile.Company.Code.Contains(request.SearchFilter)
                                                   );
            }
            var projectedList = fullListIq.ProjectTo<CashRegCatProductListDto>(_mapper.ConfigurationProvider);
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<CashRegCatProductListDto>.CreateAsync(projectedList, pageIndex, pageSize);
            var response = new IndexDataTableResponse<CashRegCatProductListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                Data = listItems
            };
            return Ok(response);
        }
        [HttpGet("GetIndexTblDataMediaEntryItems")]
        public async Task<IActionResult> GetIndexTblDataMediaEntryItems([FromQuery] IndexDataTableRequest request)
        {
            IQueryable<MediaEntry> fullListIq = _context.MediaEntries;
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {

                    case "namesort:asc":
                        fullListIq = fullListIq.OrderBy(p => p.MediaFile);
                        break;
                    case "namesort:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.MediaFile);
                        break;

                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.MediaFile.Contains(request.SearchFilter));
            }
            var projectedList = fullListIq.ProjectTo<MediaEntryDto>(_mapper.ConfigurationProvider);
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<MediaEntryDto>.CreateAsync(projectedList, pageIndex, pageSize);
            foreach (var mediaItem in listItems)
            {
                mediaItem.Url = Url.Content("~/productimages/" + mediaItem.MediaFile);
            }
            var response = new IndexDataTableResponse<MediaEntryDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                Data = listItems
            };
            return Ok(response);
        }

        [HttpPost("AssignProductsToCashCategory")]
        public async Task<IActionResult> AssignProductsToCashCategory([FromBody] CashCategoriesProductsRequest request)
        {
            //Thread.Sleep(1500);
            if (request.ProductIdList == null)
            {
                return BadRequest(new { Message = "Nothing to add" });
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                foreach (var itemId in request.ProductIdList)
                {
                    var b = await _context.CrCatWarehouseItems.SingleOrDefaultAsync(p =>
                        p.ClientProfileId == request.ClientProfileId &&
                        p.CashRegCategoryId == request.CashRegCategoryId &&
                        p.WarehouseItemId == itemId);
                    if (b == null)
                    {
                        var itemToAdd = new CrCatWarehouseItem()
                        {
                            ClientProfileId = request.ClientProfileId,
                            CashRegCategoryId = request.CashRegCategoryId,
                            WarehouseItemId = itemId
                        };
                        await _context.CrCatWarehouseItems.AddAsync(itemToAdd);
                    }
                }

                try
                {
                    var toAddCount = _context.ChangeTracker.Entries().Count(x => x.State == EntityState.Added);
                    // throw new Exception("Test error");
                    var addedCount = await _context.SaveChangesAsync();
                    transaction.Commit();
                    string message = $"Procedure completed";
                    return Ok(new { message });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    //throw;
                    return NotFound(new
                    {
                        Error = e.Message
                    });
                }
            }
        }
        [HttpPost("AssignMediaToProduct")]
        public async Task<IActionResult> AssignMediaToProduct([FromBody] MediaListProductRequest request)
        {
            int requested = 0;
            int allreadyAssigned = 0;

            // Thread.Sleep(1500);
            if (request.MediaIds == null)
            {
                return BadRequest(new { Message = "Nothing to add" });
            }
            if (request.ProductId == 0)
            {
                return BadRequest(new { Message = "Nothing to add" });
            }

            requested = request.MediaIds.Count;

            using (var transaction = _context.Database.BeginTransaction())
            {
                foreach (var mediaItemId in request.MediaIds)
                {
                    var b = await _context.ProductMedia.SingleOrDefaultAsync(p =>
                        p.MediaEntryId == mediaItemId &&
                        p.ProductId == request.ProductId);
                    if (b == null)
                    {
                        var itemToAdd = new ProductMedia()
                        {
                            ProductId = request.ProductId,
                            MediaEntryId = mediaItemId
                        };
                        await _context.ProductMedia.AddAsync(itemToAdd);
                    }
                    else
                    {
                        allreadyAssigned += 1;
                    }
                }

                try
                {
                    var toAddCount = _context.ChangeTracker.Entries().Count(x => x.State == EntityState.Added);
                    // throw new Exception("Test error");
                    var addedCount = await _context.SaveChangesAsync();
                    transaction.Commit();
                    var ms = new StringBuilder()
                        .Append("Η αντιστοίχιση εικόνων ολοκληρώθηκε")
                        .Append($"</br>Στάλθηκαν:       {requested} εικόνες")
                        .Append($"</br>Αντιστοιχισμένες:{allreadyAssigned} εικόνες")
                        .Append($"</br>Επιτυχής :       {addedCount} εικόνες");

                    string message = ms.ToString();
                    return Ok(new { message });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    //throw;
                    return NotFound(new
                    {
                        Error = e.Message
                    });
                }
            }
        }
    }

    class DataTableData
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<FinDiaryExpenseTransactionDto> Data { get; set; }
    }
}