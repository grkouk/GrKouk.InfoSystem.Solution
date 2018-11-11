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
    public class CostCentresController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public CostCentresController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/CostCentres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CostCentre>>> GetCostCentres()
        {
            return await _context.CostCentres.ToListAsync();
        }

        // GET: api/CostCentres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CostCentre>> GetCostCentre(int id)
        {
            var costCentre = await _context.CostCentres.FindAsync(id);

            if (costCentre == null)
            {
                return NotFound();
            }

            return costCentre;
        }

        // PUT: api/CostCentres/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCostCentre(int id, CostCentre costCentre)
        {
            if (id != costCentre.Id)
            {
                return BadRequest();
            }

            _context.Entry(costCentre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostCentreExists(id))
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

        // POST: api/CostCentres
        [HttpPost]
        public async Task<ActionResult<CostCentre>> PostCostCentre(CostCentre costCentre)
        {
            _context.CostCentres.Add(costCentre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCostCentre", new { id = costCentre.Id }, costCentre);
        }

        // DELETE: api/CostCentres/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CostCentre>> DeleteCostCentre(int id)
        {
            var costCentre = await _context.CostCentres.FindAsync(id);
            if (costCentre == null)
            {
                return NotFound();
            }

            _context.CostCentres.Remove(costCentre);
            await _context.SaveChangesAsync();

            return costCentre;
        }

        private bool CostCentreExists(int id)
        {
            return _context.CostCentres.Any(e => e.Id == id);
        }

        // GET: api/CostCentreSearchList
        [HttpGet("CostCentreSearchList")]
        public async Task<IActionResult> CostCentreSearchList()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var costCentreList = await _context.CostCentres.OrderBy(p => p.Name)
                .Select(s => new
                {
                    Name = s.Name,
                    Key = s.Id
                })
                .ToListAsync();

            if (costCentreList == null || costCentreList.Count == 0)
            {
                return NotFound();
            }

            return Ok(costCentreList);
        }
    }
}
