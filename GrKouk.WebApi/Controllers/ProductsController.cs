using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Domain.MediaEntities;
using GrKouk.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Dtos.MobileDtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GrKouk.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("AllProducts")]
        public async Task<ActionResult<IEnumerable<ProductListDto>>> GetAllProducts()
        {
            
            
            var items = await _context.WarehouseItems.Where(o =>  o.WarehouseItemNature == WarehouseItemNatureEnum.WarehouseItemNatureMaterial)
                .ToListAsync();
            var products = _mapper.Map<List<ProductListDto>>(items);
            foreach (var productItem in products)
            {
                try
                {
                    var productMedia = await _context.ProductMedia
                        .Include(p => p.MediaEntry)
                        .SingleOrDefaultAsync(p => p.ProductId == productItem.Id);
                    if (productMedia != null)
                    {
                        //productItem.ImageUrl = Url.Content("~/productimages/" + productMedia.MediaEntry.MediaFile);
                        productItem.ImageUrl = "http://info.villakoukoudis.com/productimages/" + productMedia.MediaEntry.MediaFile;
                    }
                    else
                    {
                        productItem.ImageUrl = "http://info.villakoukoudis.com/productimages/" + "noimage.jpg";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    productItem.ImageUrl = "http://info.villakoukoudis.com/productimages/" + "noimage.jpg";
                }

            }
            return products;
        }

        [HttpGet("GetProductsSyncList")]
        public async Task<ActionResult<IEnumerable<ProductSyncDto>>> GetProductsSyncList(string client)
        {
            if (string.IsNullOrEmpty(client))
            {
                return BadRequest();
            }

            var clientProfile = await _context.ClientProfiles.SingleOrDefaultAsync(o => o.Code == client);
            if (clientProfile == null)
            {
                return BadRequest();
            }

            var companyId = clientProfile.CompanyId;

            var items = await _context.WarehouseItems.Where(o=>o.CompanyId==companyId && o.WarehouseItemNature==WarehouseItemNatureEnum.WarehouseItemNatureMaterial)
                .ToListAsync();
            var products = _mapper.Map<List<ProductSyncDto>>(items);

            return products;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
