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
    public class RevenueCentresController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public RevenueCentresController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/RevenueCentres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RevenueCentre>>> GetRevenueCentres()
        {
            return await _context.RevenueCentres.ToListAsync();
        }

        // GET: api/RevenueCentres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RevenueCentre>> GetRevenueCentre(int id)
        {
            var revenueCentre = await _context.RevenueCentres.FindAsync(id);

            if (revenueCentre == null)
            {
                return NotFound();
            }

            return revenueCentre;
        }

        // PUT: api/RevenueCentres/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRevenueCentre(int id, RevenueCentre revenueCentre)
        {
            if (id != revenueCentre.Id)
            {
                return BadRequest();
            }

            _context.Entry(revenueCentre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RevenueCentreExists(id))
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

        // POST: api/RevenueCentres
        [HttpPost]
        public async Task<ActionResult<RevenueCentre>> PostRevenueCentre(RevenueCentre revenueCentre)
        {
            _context.RevenueCentres.Add(revenueCentre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRevenueCentre", new { id = revenueCentre.Id }, revenueCentre);
        }

        // DELETE: api/RevenueCentres/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RevenueCentre>> DeleteRevenueCentre(int id)
        {
            var revenueCentre = await _context.RevenueCentres.FindAsync(id);
            if (revenueCentre == null)
            {
                return NotFound();
            }

            _context.RevenueCentres.Remove(revenueCentre);
            await _context.SaveChangesAsync();

            return revenueCentre;
        }

        private bool RevenueCentreExists(int id)
        {
            return _context.RevenueCentres.Any(e => e.Id == id);
        }

        // GET: api/RevenueCentreSearchList
        [HttpGet("RevenueCentreSearchList")]
        public async Task<IActionResult> RevenueCentreSearchList()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var revenueCentreList = await _context.RevenueCentres.OrderBy(p => p.Name)
                .Select(s => new
                {
                    Name = s.Name,
                    Key = s.Id
                })
                .ToListAsync();

            if (revenueCentreList == null || revenueCentreList.Count == 0)
            {
                return NotFound();
            }

            return Ok(revenueCentreList);
        }
    }
}
