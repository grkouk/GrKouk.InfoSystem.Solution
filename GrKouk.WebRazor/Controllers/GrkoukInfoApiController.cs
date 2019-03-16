using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using GrKouk.InfoSystem.Dtos.WebDtos;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.DiaryTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.SellDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.TransactorTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseItems;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseTransactions;
using GrKouk.WebApi.Data;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Controllers
{
    public class IdList
    {
        public string Section { get; set; }
        public List<int> Ids { get; set; }
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
                int companyId;
                if (Int32.TryParse(request.CompanyFilter, out companyId))
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
            //return new JsonResult(response);
            return Ok(response);
        }

        [HttpGet("GetIndexTblDataBuyDocuments")]
        public async Task<IActionResult> GetIndexTblDataBuyDocuments([FromQuery] IndexDataTableRequest request)
        {
            IQueryable<BuyDocument> fullListIq = _context.BuyDocuments;

            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "transactiondate:asc":
                        fullListIq = fullListIq.OrderBy(p => p.TransDate);
                        break;
                    case "transactiondate:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.TransDate);
                        break;
                    case "transactorname:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Transactor.Name);
                        break;
                    case "transactorname:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Transactor.Name);
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
                int companyId;
                if (Int32.TryParse(request.CompanyFilter, out companyId))
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
            var t = fullListIq.ProjectTo<BuyDocListDto>(_mapper.ConfigurationProvider);
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<BuyDocListDto>.CreateAsync(t, pageIndex, pageSize);
            decimal sumAmountTotal = listItems.Sum(p => p.TotalAmount);

            var response = new IndexDataTableResponse<BuyDocListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                SumOfAmount = sumAmountTotal,
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
                    case "transactiondate:asc":
                        fullListIq = fullListIq.OrderBy(p => p.TransDate);
                        break;
                    case "transactiondate:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.TransDate);
                        break;
                    case "transactorname:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Transactor.Name);
                        break;
                    case "transactorname:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Transactor.Name);
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
                int companyId;
                if (Int32.TryParse(request.CompanyFilter, out companyId))
                {
                    if (companyId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.CompanyId == companyId);
                    }
                }
            }

            var t = fullListIq.ProjectTo<SellDocListDto>(_mapper.ConfigurationProvider);
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<SellDocListDto>.CreateAsync(t, pageIndex, pageSize);
            decimal sumAmountTotal = listItems.Sum(p => p.TotalAmount);

            var response = new IndexDataTableResponse<SellDocListDto>
            {
                TotalRecords = listItems.TotalCount,
                TotalPages = listItems.TotalPages,
                HasPrevious = listItems.HasPrevious,
                HasNext = listItems.HasNext,
                SumOfAmount = sumAmountTotal,
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
                    case "transactiondate:asc":
                        fullListIq = fullListIq.OrderBy(p => p.TransDate);
                        break;
                    case "transactiondate:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.TransDate);
                        break;
                    case "transactorname:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Transactor.Name);
                        break;
                    case "transactorname:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Transactor.Name);
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
                int companyId;
                if (Int32.TryParse(request.CompanyFilter, out companyId))
                {
                    if (companyId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.CompanyId == companyId);
                    }
                }
            }

            var t = fullListIq.ProjectTo<TransactorTransListDto>(_mapper.ConfigurationProvider);
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<TransactorTransListDto>.CreateAsync(t, pageIndex, pageSize);
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
                    case "transactiondate:asc":
                        fullListIq = fullListIq.OrderBy(p => p.TransDate);
                        break;
                    case "transactiondate:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.TransDate);
                        break;
                    case "transactorname:asc":
                        fullListIq = fullListIq.OrderBy(p => p.WarehouseItem.Name);
                        break;
                    case "transactorname:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.WarehouseItem.Name);
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
                int companyId;
                if (Int32.TryParse(request.CompanyFilter, out companyId))
                {
                    if (companyId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.CompanyId == companyId);
                    }
                }
            }

            var t = fullListIq.ProjectTo<WarehouseTransListDto>(_mapper.ConfigurationProvider);
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<WarehouseTransListDto>.CreateAsync(t, pageIndex, pageSize);
            //decimal sumAmountTotal = listItems.Sum(p => p.TotalAmount);
            //decimal sumDebit = listItems.Sum(p => p.DebitAmount);
            //decimal sumCredit = listItems.Sum(p => p.CreditAmount);
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
                // SumOfAmount = sumAmountTotal,
                // SumOfDebit = sumDebit,
                // SumOfCredit = sumCredit,
                Data = listItems
            };
            //return new JsonResult(response);
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
            if (!String.IsNullOrEmpty(request.SortData))
            {
                switch (request.SortData.ToLower())
                {
                    case "transactiondate:asc":
                        transactionsList = transactionsList.OrderBy(p => p.TransDate);
                        break;
                    case "transactiondate:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.TransDate);
                        break;
                    case "transactorname:asc":
                        transactionsList = transactionsList.OrderBy(p => p.Transactor.Name);
                        break;
                    case "transactorname:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.Transactor.Name);
                        break;

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
                int companyId;
                if (Int32.TryParse(request.CompanyFilter, out companyId))
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
                g.TransactorId
            }
                )
                .Select(s => new
                {
                    Id = s.Key.TransactorId,
                    TransactorName = "",
                    CompanyCode = s.Key.CompanyCode,
                    DebitAmount = s.Sum(x => x.DebitAmount),
                    CreditAmount = s.Sum(x => x.CreditAmount)
                }).ToList();

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
        [HttpGet("GetIndexTblDataTransactors")]
        public async Task<IActionResult> GetIndexTblDataTransactors([FromQuery] IndexDataTableRequest request)
        {
            IQueryable<Transactor> fullListIq = _context.Transactors;
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

                    case "transactorname:asc":
                        fullListIq = fullListIq.OrderBy(p => p.Name);
                        break;
                    case "transactorname:desc":
                        fullListIq = fullListIq.OrderByDescending(p => p.Name);
                        break;

                }
            }

            if (!String.IsNullOrEmpty(request.CompanyFilter))
            {
                int companyId;
                if (Int32.TryParse(request.CompanyFilter, out companyId))
                {
                    if (companyId > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.CompanyId == companyId);
                    }
                }
            }
            if (!String.IsNullOrEmpty(request.SearchFilter))
            {
                fullListIq = fullListIq.Where(p => p.Name.Contains(request.SearchFilter)
                                                   || p.EMail.Contains(request.SearchFilter)
                                                   || p.Address.Contains(request.SearchFilter)
                                                   || p.City.Contains(request.SearchFilter)
                                                   || p.PhoneFax.Contains(request.SearchFilter)
                                                   || p.PhoneMobile.Contains(request.SearchFilter)
                                                   || p.PhoneWork.Contains(request.SearchFilter));
            }
            var projectedList = fullListIq.ProjectTo<TransactorListDto>(_mapper.ConfigurationProvider);
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<TransactorListDto>.CreateAsync(projectedList, pageIndex, pageSize);

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
        [HttpGet("GetIndexTblDataWarehouseItems")]
        public async Task<IActionResult> GetIndexTblDataWarehouseItems([FromQuery] IndexDataTableRequest request)
        {
            IQueryable<WarehouseItem> fullListIq = _context.WarehouseItems;
            int warehouseItemNatureFilter = 0;
            if (!String.IsNullOrEmpty(request.WarehouseItemNatureFilter))
            {
                if (Int32.TryParse(request.WarehouseItemNatureFilter, out warehouseItemNatureFilter))
                {
                    if (warehouseItemNatureFilter > 0)
                    {
                        fullListIq = fullListIq.Where(p => p.WarehouseItemNature == (WarehouseItemNatureEnum) warehouseItemNatureFilter);
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
                        fullListIq = fullListIq.Where(p => p.CompanyId == companyId);
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
            var projectedList = fullListIq.ProjectTo<WarehouseItemListDto>(_mapper.ConfigurationProvider);
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<WarehouseItemListDto>.CreateAsync(projectedList, pageIndex, pageSize);

            //var relevantDiarys = new List<SearchListItem>();
            //var dList = await _context.DiaryDefs.Where(p => p.DiaryType == DiaryTypeEnum.DiaryTypeEnumTransactors)
            //    .ToListAsync();
            //if (warehouseItemNatureFilter != 0)
            //{
            //    var tf = warehouseItemNatureFilter;
            //    relevantDiarys = dList
            //        .Where(tx => Array.ConvertAll(tx.SelectedTransTypes.Split(","), int.Parse).Contains(tf))
            //        .Select(item => new SearchListItem()
            //        {
            //            Value = item.Id,
            //            Text = item.Name
            //        })
            //        .ToList();
            //}

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
    }

    class DataTableData
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<FinDiaryExpenseTransactionDto> Data { get; set; }
    }
}