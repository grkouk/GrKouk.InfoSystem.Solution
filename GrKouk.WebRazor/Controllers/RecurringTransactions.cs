using System;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Definitions;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.SellDocuments;
using GrKouk.WebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using NToastNotify.Helpers;

namespace GrKouk.WebRazor.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    [ApiController]
    public class RecurringTransactions : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        
        public RecurringTransactions(ApiDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet(template: "CreateDocFromRecTrans")]
        public async Task<IActionResult> CreateDocFromRecTrans(int id)
        {
            if (id<=0)
            {
                return BadRequest(error: new {Message = "Id must be grater than 0"});
            }

            var recDef = await _context.RecurringTransDocs
                .Include(p=>p.DocLines)
                .SingleOrDefaultAsync(p => p.Id == id);
            if (recDef==null)
            {
                return NotFound(
                    new
                    {
                        Message = "Recurring transaction definition not found"
                    });
            }

            IActionResult actionResult;
            switch (recDef.RecurringDocType)
            {
                case RecurringDocTypeEnum.BuyType:
                    var buyDocDto = _mapper.Map<BuyDocCreateAjaxDto>(recDef);
                    var buyUpdController = new MaterialsController(_context,_mapper);
                    actionResult = await buyUpdController.PostMaterialBuyDoc(buyDocDto);
                    break;
                case RecurringDocTypeEnum.SellType:
                    var sellDocDto = _mapper.Map<SellDocCreateAjaxDto>(recDef);
                    var sellUpdController = new MaterialsController(_context,_mapper);
                    actionResult = await sellUpdController.PostSalesDoc(sellDocDto);
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var rsJson = actionResult.ToJson();
            return Ok(actionResult);
        }
        
    }
}