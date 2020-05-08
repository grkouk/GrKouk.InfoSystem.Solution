using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.TransactorTransactions;
using GrKouk.WebApi.Data;
using GrKouk.WebRazor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebRazor.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialDataController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public FinancialDataController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        private decimal ConvertAmount(int companyCurrencyId,int displayCurrencyId,IList<ExchangeRate> rates,decimal amount)
        {
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
        [HttpGet("GetTransactorFinancialSummaryData")]
        public async Task<IActionResult> GetTransactorFinancialSummaryData([FromQuery] IndexDataTableRequest request)
        {
           
            if (request.TransactorId<=0)
            {
                return BadRequest();
            }
            IQueryable<TransactorTransaction> fullListIq = _context.TransactorTransactions.Where(p => p.TransactorId == request.TransactorId);
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
            
            var currencyRates = await _context.ExchangeRates.OrderByDescending(p => p.ClosingDate)
                .Take(10)
                .ToListAsync();
            var t = fullListIq.ProjectTo<TransactorTransListDto>(_mapper.ConfigurationProvider);
            var t1 =await t.Select(p => new TransactorTransListDto
            {
                TransTransactorDocSeriesId = p.TransTransactorDocSeriesId,
                TransTransactorDocSeriesName = p.TransTransactorDocSeriesName,
                TransTransactorDocSeriesCode = p.TransTransactorDocSeriesCode,
                TransTransactorDocTypeId = p.TransTransactorDocTypeId,
                FinancialAction = p.FinancialAction,
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
            var transactor = await _context.Transactors.Include(p => p.TransactorType)
                .FirstOrDefaultAsync(p => p.Id == request.TransactorId);
            if (transactor==null )
            {
                return BadRequest();
            }
            await _context.Entry(transactor).Reference(p => p.TransactorType).LoadAsync();
            var transactorType = transactor.TransactorType;
            if (transactorType == null)
            {
                return BadRequest();
            }

            decimal difference = 0;
            switch (transactorType.Code)
            {
                case "SYS.CUSTOMER":
                    difference = grandSumOfDebit - grandSumOfCredit;
                    break;
                case "SYS.SUPPLIER":
                    difference = grandSumOfCredit - grandSumOfDebit;
                    break;
                case "SYS.DEPARTMENT":
                    difference = grandSumOfDebit - grandSumOfCredit;
                    break;
                case "SYS.DTRANSACTOR":
                    difference = grandSumOfCredit - grandSumOfDebit;
                    break;
                default:
                    break;
            }
            var response = new TransactorFinancialDataResponse()
            {
                SumOfDebit = grandSumOfDebit,
                SumOfCredit = grandSumOfCredit,
                SumOfDifference = difference
            };
            return Ok(response);
        }
    }
}