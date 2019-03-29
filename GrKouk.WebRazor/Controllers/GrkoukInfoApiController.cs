using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.MediaEntities;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using GrKouk.InfoSystem.Dtos.WebDtos;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.CashRegister;
using GrKouk.InfoSystem.Dtos.WebDtos.DiaryTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.Media;
using GrKouk.InfoSystem.Dtos.WebDtos.SellDocuments;
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
                g.CompanyCode,
                g.WarehouseItemId
            }
                )
                .Select(s => new
                {
                    Id = s.Key.WarehouseItemId,
                    MaterialName = "",
                    CompanyCode = s.Key.CompanyCode,
                    ImportVolume = s.Sum(x => x.ImportUnits),
                    ExportVolume = s.Sum(x => x.ExportUnits),
                    ImportValue = s.Sum(x => x.ImportAmount),
                    ExportValue = s.Sum(x => x.ExportAmount)
                }).ToList();

            var isozigioType = "FREE";
            var isozigioName = "";


            switch (warehouseItemNatureFilter)
            {
                case WarehouseItemNatureEnum.WarehouseItemNatureUndefined:
                    isozigioName = "";
                    break;
                case WarehouseItemNatureEnum.WarehouseItemNatureMaterial:
                    isozigioName = "Υλικών";
                    isozigioType = "SUPPLIER";
                    break;
                case WarehouseItemNatureEnum.WarehouseItemNatureService:
                    isozigioName = "Υπηρεσιών";
                    isozigioType = "SUPPLIER";
                    break;
                case WarehouseItemNatureEnum.WarehouseItemNatureExpense:
                    isozigioName = "Δαπάνων";
                    isozigioType = "SUPPLIER";
                    break;
                case WarehouseItemNatureEnum.WarehouseItemNatureIncome:
                    isozigioName = "Εσόδων";
                    isozigioType = "SUPPLIER";
                    break;
                case WarehouseItemNatureEnum.WarehouseItemNatureFixedAsset:
                    isozigioName = "Παγίων";
                    isozigioType = "SUPPLIER";
                    break;
                case WarehouseItemNatureEnum.WarehouseItemNatureRawMaterial:
                    isozigioName = "Πρώτων Υλών";
                    isozigioType = "SUPPLIER";
                    break;
                default:
                    isozigioName = "";
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
            decimal sumDifference = 0;

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
                        fullListIq = fullListIq.Where(p => p.WarehouseItemNature == (WarehouseItemNatureEnum)warehouseItemNatureFilter);
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
            foreach (var productItem in listItems)
            {
                var productMedia = await _context.ProductMedia
                    .Include(p=>p.MediaEntry)
                    .SingleOrDefaultAsync(p => p.ProductId == productItem.Id);
                if (productMedia!=null)
                {
                    productItem.Url = Url.Content("~/productimages/" + productMedia.MediaEntry.MediaFile);
                }
                
            }
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
                    case "datesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.TransDate);
                        break;
                    case "datesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.TransDate);
                        break;
                    case "namesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.Transactor.Name);
                        break;
                    case "namesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.Transactor.Name);
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

            var t = transactionsList.ProjectTo<TransactorTransListDto>(_mapper.ConfigurationProvider);
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
                    case "namesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.Transactor.Name);
                        break;
                    case "namesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.Transactor.Name);
                        break;

                }
            }
            #region CommentOut
            if (!String.IsNullOrEmpty(request.DateRange))
            {
                var datePeriodFilter = request.DateRange;
                DateFilterDates dfDates = DateFilter.GetDateFilterDates(datePeriodFilter);
                DateTime fromDate = dfDates.FromDate;
                DateTime toDate = dfDates.ToDate;

                transactionsList = transactionsList.Where(p => p.TransDate >= fromDate && p.TransDate <= toDate);
            }
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
            var dbTransactions = await dbTrans.ToListAsync();

            var listWithTotal = new List<KartelaLine>();

            decimal runningTotal = 0;
            foreach (var dbTransaction in dbTransactions)
            {
                switch (transactorType.Code)
                {
                    case "SYS.DTRANSACTOR":

                        break;
                    case "SYS.CUSTOMER":
                        runningTotal = dbTransaction.DebitAmount - dbTransaction.CreditAmount;
                        break;
                    case "SYS.SUPPLIER":
                        runningTotal = dbTransaction.CreditAmount - dbTransaction.DebitAmount;
                        break;
                    default:
                        runningTotal = dbTransaction.CreditAmount - dbTransaction.DebitAmount;
                        break;
                }


                listWithTotal.Add(new KartelaLine
                {
                    TransDate = dbTransaction.TransDate,
                    DocSeriesCode = dbTransaction.TransTransactorDocSeriesCode,
                    RefCode = dbTransaction.TransRefCode,
                    CompanyCode = dbTransaction.CompanyCode,
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
                    case "namesort:asc":
                        transactionsList = transactionsList.OrderBy(p => p.WarehouseItem.Name);
                        break;
                    case "namesort:desc":
                        transactionsList = transactionsList.OrderByDescending(p => p.WarehouseItem.Name);
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
                    }
                }
            }

            var dbTrans = transactionsList.ProjectTo<WarehouseTransListDto>(_mapper.ConfigurationProvider);
            var dbTransactions = await dbTrans.ToListAsync();

            var dbTransBeforePeriod = transListBeforePeriod.ProjectTo<WarehouseTransListDto>(_mapper.ConfigurationProvider);

            //Create before period line
            var bl1 = new
            {
                ImportVolume = dbTransBeforePeriod.Sum(x => x.ImportUnits),
                ExportVolume = dbTransBeforePeriod.Sum(x => x.ExportUnits),

                ImportValue = dbTransBeforePeriod.Sum(x => x.ImportAmount),
                ExportValue = dbTransBeforePeriod.Sum(x => x.ExportAmount)
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
            decimal sumDifference = 0;

            IQueryable<WarehouseKartelaLine> fullListIq = from s in outList select s;
            //if (!String.IsNullOrEmpty(request.SearchFilter))
            //{
            //    fullListIq = fullListIq.Where(p => 
            //        //p.MaterialName.Contains(request.SearchFilter)
            //     p.RefCode.Contains(request.SearchFilter)
            //    //|| p.CompanyCode.Contains(request.SearchFilter)
            //    //|| p.TransDate.ToString().Contains(request.SearchFilter)
            //    //|| p.DocSeriesCode.Contains(request.SearchFilter)
            //        );
            //}
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
                        p.ProductId == request.ProductId );
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