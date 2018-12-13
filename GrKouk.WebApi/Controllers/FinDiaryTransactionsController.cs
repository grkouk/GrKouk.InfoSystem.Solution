using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinDiaryTransactionsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public FinDiaryTransactionsController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/FinDiaryTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinDiaryTransaction>>> GetFinDiaryTransactions()
        {
            return await _context.FinDiaryTransactions.ToListAsync();
        }

        // GET: api/FinDiaryTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FinDiaryTransaction>> GetFinDiaryTransaction(int id)
        {
            var finDiaryTransaction = await _context.FinDiaryTransactions.FindAsync(id);

            if (finDiaryTransaction == null)
            {
                return NotFound();
            }

            return finDiaryTransaction;
        }

        // PUT: api/FinDiaryTransactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinDiaryTransaction(int id, FinDiaryTransaction finDiaryTransaction)
        {
            if (id != finDiaryTransaction.Id)
            {
                return BadRequest();
            }

            _context.Entry(finDiaryTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinDiaryTransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FinDiaryTransactions
        [HttpPost]
        public async Task<ActionResult<FinDiaryTransaction>> PostFinDiaryTransaction(FinDiaryTransaction finDiaryTransaction)
        {
            _context.FinDiaryTransactions.Add(finDiaryTransaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFinDiaryTransaction", new { id = finDiaryTransaction.Id }, finDiaryTransaction);
        }

        // DELETE: api/FinDiaryTransactions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FinDiaryTransaction>> DeleteFinDiaryTransaction(int id)
        {
            var finDiaryTransaction = await _context.FinDiaryTransactions.FindAsync(id);
            if (finDiaryTransaction == null)
            {
                return NotFound();
            }

            _context.FinDiaryTransactions.Remove(finDiaryTransaction);
            await _context.SaveChangesAsync();

            return finDiaryTransaction;
        }

        private bool FinDiaryTransactionExists(int id)
        {
            return _context.FinDiaryTransactions.Any(e => e.Id == id);
        }
    }
}
