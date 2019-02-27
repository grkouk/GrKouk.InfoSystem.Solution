using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
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
    [Authorize]
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
            //Thread.Sleep(1500);
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



        [HttpGet("GetTblData")]
        public async Task<IActionResult> GetTblData2([FromQuery] DataTableParameters request)
        {
            List<DataOrder> orderColumns = new List<DataOrder>();
            List<DataTableColumn> dataTableColumns = new List<DataTableColumn>();

            if (request.Order != null)
            {
                var orderData = request.Order;

                foreach (var orderItem in orderData)
                {
                    var orderColumn = new DataOrder();

                    foreach (var keyValuePair in orderItem)
                    {


                        string k = keyValuePair.Key;
                        string v = keyValuePair.Value;
                        switch (k)
                        {
                            case "column":
                                orderColumn.Column = Int32.Parse(v);
                                break;
                            case "dir":
                                orderColumn.Dir = v;
                                break;
                        }

                    }
                    orderColumns.Add(orderColumn);
                }

            }

            if (request.Columns != null)
            {
                var colList = request.Columns;

                foreach (var colItem in colList)
                {
                    // var bCreateColumn = true;
                    DataTableColumn colData = new DataTableColumn();
                    foreach (var keyValuePair in colItem)
                    {

                        string k = keyValuePair.Key;
                        string v = keyValuePair.Value;
                        switch (k)
                        {
                            case "name":
                                colData.Name = v;
                                break;
                            case "searchable":
                                colData.Searchable = Boolean.Parse(v);

                                break;
                            case "orderable":
                                colData.Orderable = Boolean.Parse(v);
                                break;
                            default:
                                break;
                        }

                        // if (!ignoreCol)
                        //{

                        //}

                    }
                    dataTableColumns.Add(colData);
                }
            }
            IQueryable<FinDiaryTransaction> expensesIq = from s in _context.FinDiaryTransactions
                                                         select s;

            expensesIq = expensesIq.Include(f => f.Company)
                .Include(f => f.CostCentre)
                .Include(f => f.FinTransCategory)
                .Include(f => f.Transactor);
            if (orderColumns.Count > 0)
            {
                foreach (var orderColumn in orderColumns)
                {
                    var colIdx = orderColumn.Column;
                    var orderType = orderColumn.Dir;
                    var colDef = dataTableColumns[colIdx];
                    switch (colDef.Name.ToLower())
                    {
                        case "transactiondate":
                            expensesIq = orderType == "desc" ? expensesIq.OrderByDescending(p => p.TransactionDate) : expensesIq.OrderBy(p => p.TransactionDate);
                            break;
                        case "transactorname":
                            expensesIq = orderType == "desc" ? expensesIq.OrderByDescending(p => p.Transactor.Name) : expensesIq.OrderBy(p => p.Transactor.Name);
                            break;
                        case "companyname":
                            expensesIq = orderType == "desc" ? expensesIq.OrderByDescending(p => p.Company.Name) : expensesIq.OrderBy(p => p.Company.Name);
                            break;
                        case "referencecode":
                            expensesIq = orderType == "desc" ? expensesIq.OrderByDescending(p => p.ReferenceCode) : expensesIq.OrderBy(p => p.ReferenceCode);
                            break;
                    }
                }
            }

            var datePeriodFilter = request.DateRange;
            DateFilterDates dfDates = DateFilter.GetDateFilterDates(datePeriodFilter);
            DateTime fromDate = dfDates.FromDate;
            DateTime toDate = dfDates.ToDate;


            expensesIq = expensesIq.Where(p => p.TransactionDate >= fromDate && p.TransactionDate <= toDate);




            var t = expensesIq.ProjectTo<FinDiaryExpenseTransactionDto>(_mapper.ConfigurationProvider);
            var itemsToSkip = request.Start;

            var pageSize = request.Length;

            var listItems = await PagedList<FinDiaryExpenseTransactionDto>.CreateForDataTableAsync(t, itemsToSkip, pageSize);


            // var response = MyDataTablesResponse.Create(request.Draw, listItems.TotalCount, listItems.TotalCount, listItems);
            var response = new DataTableData
            {
                Draw = request.Draw,
                RecordsTotal = listItems.TotalCount,
                RecordsFiltered = listItems.TotalCount,
                Data = listItems
            };

            return new JsonResult(response);
        }

        [HttpGet("GetIndexTblData")]
        public async Task<IActionResult> GetIndexTblData([FromQuery] IndexDataTableRequest request)
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
                if (Int32.TryParse(request.CompanyFilter,out companyId))
                {
                    if (companyId>0)
                    {
                        expensesIq = expensesIq.Where(p => p.CompanyId == companyId);
                    }
                    
                }
               

            }

            var t = expensesIq.ProjectTo<FinDiaryExpenseTransactionDto>(_mapper.ConfigurationProvider);
            var pageIndex = request.PageIndex;

            var pageSize = request.PageSize;

            var listItems = await PagedList<FinDiaryExpenseTransactionDto>.CreateAsync(t, pageIndex, pageSize);
            decimal sumAmountTotal = listItems.Sum(p => p.AmountTotal);

            var response = new IndexDataTableResponse
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
    }

    class DataTableData
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<FinDiaryExpenseTransactionDto> Data { get; set; }
    }
}