using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Dtos;
using GrKouk.InfoSystem.Domain;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Transactions")]
    public class TransactionsController : Controller
    {
        private readonly ApiDbContext _context;

        public TransactionsController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public IEnumerable<FinDiaryTransactionDto> GetTransactions()
        {
            var transactions = Mapper.Map<IEnumerable<FinDiaryTransactionDto>>(
                _context.FinDiaryTransactions
                    .Include(s => s.Transactor)
                    .Include(t => t.FinTransCategory)
                    .Include(t => t.Company)
                    .Include(t => t.CostCentre)
                    .Include(t => t.RevenueCentre)
                    .ToList());
            return transactions;
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = await _context.FinDiaryTransactions.SingleOrDefaultAsync(m => m.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }
        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction([FromRoute] int id, [FromBody] FinDiaryTransactionModifyDto transactionModifyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transactionModifyDto.Id)
            {
                return BadRequest();
            }
            var entityToMap = Mapper.Map<FinDiaryTransaction>(transactionModifyDto);
            _context.Entry(entityToMap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                #region Refresh
                if (entityToMap.Transactor == null)
                {
                    _context.Entry(entityToMap).Reference(p => p.Transactor).Load();
                }
                if (entityToMap.FinTransCategory == null)
                {
                    _context.Entry(entityToMap).Reference(p => p.FinTransCategory).Load();
                }
                if (entityToMap.Company == null)
                {
                    _context.Entry(entityToMap).Reference(p => p.Company).Load();
                }
                if (entityToMap.CostCentre == null)
                {
                    _context.Entry(entityToMap).Reference(p => p.CostCentre).Load();
                }
                if (entityToMap.RevenueCentre == null)
                {
                    _context.Entry(entityToMap).Reference(p => p.RevenueCentre).Load();
                }
                #endregion
                var entityToReturn = Mapper.Map<FinDiaryTransactionModifyDto>(entityToMap);
                return Ok(entityToReturn);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!TransactionExists(id))
                {
                    return NotFound(e.ToString());
                }
                else
                {
                    throw e;
                }
            }

            return NoContent();
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] FinDiaryTransactionCreateDto transactionCreateDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var entityToMap = Mapper.Map<FinDiaryTransaction>(transactionCreateDto);
                _context.FinDiaryTransactions.Add(entityToMap);
                await _context.SaveChangesAsync();

                if (entityToMap.Transactor == null)
                {
                    _context.Entry(entityToMap).Reference(p => p.Transactor).Load();
                }
                if (entityToMap.FinTransCategory == null)
                {
                    _context.Entry(entityToMap).Reference(p => p.FinTransCategory).Load();
                }
                if (entityToMap.Company == null)
                {
                    _context.Entry(entityToMap).Reference(p => p.Company).Load();
                }
                if (entityToMap.CostCentre == null)
                {
                    _context.Entry(entityToMap).Reference(p => p.CostCentre).Load();
                }
                if (entityToMap.RevenueCentre == null)
                {
                    _context.Entry(entityToMap).Reference(p => p.RevenueCentre).Load();
                }
                var entityToReturn = Mapper.Map<FinDiaryTransactionDto>(entityToMap);
                return CreatedAtAction("GetTransaction", new { id = entityToMap.Id }, entityToReturn);
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = await _context.FinDiaryTransactions.SingleOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.FinDiaryTransactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return Ok(transaction);
        }

        private bool TransactionExists(int id)
        {
            return _context.FinDiaryTransactions.Any(e => e.Id == id);
        }
        // GET: api/Transactions/TransactionsInPeriod?fromDate=2018-01-01&toDate=2018-12-31
        //[Route("api/TransactionsInPeriod")]
        [HttpGet("TransactionsInPeriod")]
        public async Task<IActionResult> GetTransactionsInPeriod(DateTime fromDate, DateTime toDate)
        {
            if (fromDate == null)
            {
                throw new ArgumentNullException();
            }
            if (toDate == null)
            {
                throw new ArgumentNullException();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // DateTime tDate=

            var transactions = Mapper.Map<IEnumerable<FinDiaryTransactionDto>>(await _context.FinDiaryTransactions.Where(m => m.TransactionDate >= fromDate && m.TransactionDate <= toDate)
                .Include(s => s.Transactor)
                .Include(t => t.FinTransCategory)
                .Include(t => t.Company)
                .Include(t => t.CostCentre)
                .Include(t => t.RevenueCentre)
                .ToListAsync());

            if (transactions == null)
            {
                return NoContent();
            }

            return Ok(transactions);
        }
    }
}
