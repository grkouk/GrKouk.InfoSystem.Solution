using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.WebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            Thread.Sleep(1500);
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
    }
}