using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyMaterialsDocs;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public MaterialsController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/Materials
        [HttpGet]
        public IEnumerable<Material> GetMaterials()
        {
            return _context.Materials;
        }

        // GET: api/Materials/5
        [HttpGet("search")]
        public async Task<IActionResult> GetMaterial( string term)
        {
            
            var materials = await _context.Materials.Where(p=>p.Name.Contains(term))
                .Select(p=>new {label=p.Name,value=p.Id}).ToListAsync();

            if (materials == null)
            {
                return NotFound();
            }

            return Ok(materials);
        }
        [HttpGet("materialdata")]
        public async Task<IActionResult> GetMaterialData(int materialId)
        {
            var materialData = await _context.Materials.Where(p => p.Id== materialId)
                .Select(p => new {
                    mainUnit =p.MainMeasureUnit.Code,
                    buyUnit =p.BuyMeasureUnit.Code,
                    factor=p.BuyUnitToMainRate,
                    fpaRate=p.FpaDef.Rate
                }).FirstOrDefaultAsync();

            if (materialData==null)
            {
                return NotFound();
            }

            return Ok(materialData);
        }

        [HttpPost("MaterialBuyDoc")]
        public async Task<IActionResult> PostMaterialBuyDoc([FromBody] BuyMaterialsDocCreateAjaxDto data)
        {
            foreach (var dataBuyDocLine in data.BuyDocLines)
            {
                Debug.WriteLine("Lines ");
                Debug.WriteLine(dataBuyDocLine.MaterialId.ToString());
                Debug.WriteLine(dataBuyDocLine.Amount.ToString());
                Debug.WriteLine(dataBuyDocLine.Q1.ToString());
                Debug.WriteLine(dataBuyDocLine.Price.ToString());
            }
            return Ok(new { });
        }
        
    }
}