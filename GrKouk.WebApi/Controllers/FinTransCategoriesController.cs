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
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FinTransCategoriesController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public FinTransCategoriesController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/FinTransCategories
        [HttpGet]
        public IEnumerable<FinTransCategory> GetFinTransCategories()
        {
            return _context.FinTransCategories;
        }

        // GET: api/FinTransCategories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFinTransCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var finTransCategory = await _context.FinTransCategories.FindAsync(id);

            if (finTransCategory == null)
            {
                return NotFound();
            }

            return Ok(finTransCategory);
        }

        // PUT: api/FinTransCategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinTransCategory([FromRoute] int id, [FromBody] FinTransCategory finTransCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != finTransCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(finTransCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinTransCategoryExists(id))
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

        // POST: api/FinTransCategories
        [HttpPost]
        public async Task<IActionResult> PostFinTransCategory([FromBody] FinTransCategory finTransCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FinTransCategories.Add(finTransCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFinTransCategory", new { id = finTransCategory.Id }, finTransCategory);
        }

        // DELETE: api/FinTransCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinTransCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var finTransCategory = await _context.FinTransCategories.FindAsync(id);
            if (finTransCategory == null)
            {
                return NotFound();
            }

            _context.FinTransCategories.Remove(finTransCategory);
            await _context.SaveChangesAsync();

            return Ok(finTransCategory);
        }

        private bool FinTransCategoryExists(int id)
        {
            return _context.FinTransCategories.Any(e => e.Id == id);
        }

        [HttpGet("FinTransCategoriesSearchList")]
        public async Task<IActionResult> FinTransCategoriesSearchList()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var CategoriesList = await _context.FinTransCategories.OrderBy(p => p.Name)
                .Select(s => new
                {
                    Name = s.Name,
                    Key = s.Id
                })
                .ToListAsync();

            if (CategoriesList == null || CategoriesList.Count == 0)
            {
                return NotFound();
            }

            return Ok(CategoriesList);
        }
    }
}