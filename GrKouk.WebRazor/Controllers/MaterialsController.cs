using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyMaterialsDocs;
using GrKouk.WebApi.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Xml;

namespace GrKouk.WebRazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public MaterialsController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Materials
        [HttpGet]
        public IEnumerable<Material> GetMaterials()
        {
            return _context.Materials;
        }

        // GET: api/Materials/5
        [HttpGet("search")]
        public async Task<IActionResult> GetMaterial(string term)
        {

            var materials = await _context.Materials.Where(p => p.Name.Contains(term))
                .Select(p => new { label = p.Name, value = p.Id }).ToListAsync();

            if (materials == null)
            {
                return NotFound();
            }

            return Ok(materials);
        }
        [HttpGet("materialdata")]
        public async Task<IActionResult> GetMaterialData(int materialId)
        {
            var lastPr = await _context.WarehouseTransactions.Where(m => m.Id == materialId)
                .Select(k => new
                {
                    LastPrice = k.AmountNet
                }).FirstOrDefaultAsync();

            var lastPrice = lastPr?.LastPrice ?? 0;

            var materialData = await _context.Materials.Where(p => p.Id == materialId && p.Active)
                .Select(p => new
                {
                    mainUnitId = p.MainMeasureUnitId,
                    secUnitId = p.SecondaryMeasureUnitId,
                    buyUnitId = p.BuyMeasureUnitId,
                    mainUnitCode = p.MainMeasureUnit.Code,
                    secUnitCode = p.SecondaryMeasureUnit.Code,
                    buyUnitCode = p.BuyMeasureUnit.Code,
                    factorSeq = p.SecondaryUnitToMainRate,
                    factorBuy = p.BuyUnitToMainRate,
                    fpaId = p.FpaDefId,
                    lastPrice = lastPrice,
                    fpaRate = p.FpaDef.Rate
                }).FirstOrDefaultAsync();

            if (materialData == null)
            {
                return NotFound();
            }

            return Ok(materialData);
        }

        [HttpPost("MaterialBuyDoc")]
        public async Task<IActionResult> PostMaterialBuyDoc([FromBody] BuyMaterialsDocCreateAjaxDto data)
        {
            const string sectionCode = "SYS-BUY-MATERIALS-SCN";

            var transToAttachNoLines = _mapper.Map<BuyMaterialsDocCreateAjaxNoLinesDto>(data);
            var transToAttach = _mapper.Map<BuyMaterialsDocument>(transToAttachNoLines);
            using (var transaction = _context.Database.BeginTransaction())
            {

                #region Section Management

                var section = await _context.Sections.SingleOrDefaultAsync(s => s.SystemName == sectionCode);
                if (section == null)
                {
                    transaction.Rollback();
                    ModelState.AddModelError(string.Empty, "Δεν υπάρχει το Section");
                    return NotFound(new
                    {
                        error = "Could not locate section "
                    });
                }

                #endregion

                var docSeries = await
                    _context.BuyMaterialDocSeriesDefs.SingleOrDefaultAsync(m => m.Id == data.MaterialDocSeriesId);

                if (docSeries is null)
                {
                    transaction.Rollback();
                    ModelState.AddModelError(string.Empty, "Δεν βρέθηκε η σειρά παραστατικού");
                    return NotFound(new
                    {
                        error = "Buy Doc Series not found"
                    });
                }

                await _context.Entry(docSeries).Reference(t => t.BuyMaterialDocTypeDef).LoadAsync();
                var docTypeDef = docSeries.BuyMaterialDocTypeDef;
                await _context.Entry(docTypeDef)
                      .Reference(t => t.TransSupplierDef)
                      .LoadAsync();
                var transSupplierDef = docTypeDef.TransSupplierDef;
                await _context.Entry(transSupplierDef)
                    .Reference(t => t.CreditTrans)
                    .LoadAsync();

                await _context.Entry(transSupplierDef)
                    .Reference(t => t.DebitTrans)
                    .LoadAsync();

                await _context.Entry(transSupplierDef)
                    .Reference(t => t.TransSupplierDefaultDocSeries)
                    .LoadAsync();

                var transSupDefaultSeries = transSupplierDef.TransSupplierDefaultDocSeries;
                var creditTrans = transSupplierDef.CreditTrans;
                var debitTrans = transSupplierDef.DebitTrans;

                var spSupplierTransaction = _mapper.Map<SupplierTransaction>(data);
                spSupplierTransaction.SectionId = section.Id;
                spSupplierTransaction.TransSupplierDocTypeId = transSupDefaultSeries.TransSupplierDocTypeDefId;
                spSupplierTransaction.TransSupplierDocSeriesId = transSupDefaultSeries.Id;
                spSupplierTransaction.FiscalPeriodId = 1;
                if (creditTrans.Action == "=" && debitTrans.Action != "=")
                {
                    spSupplierTransaction.TransactionType = FinancialTransactionTypeEnum.FinancialTransactionTypeDebit;
                    switch (debitTrans.Action)
                    {
                        case "+":

                            break;
                        case "-":
                            spSupplierTransaction.AmountNet = spSupplierTransaction.AmountNet * -1;
                            spSupplierTransaction.AmountFpa = spSupplierTransaction.AmountFpa * -1;
                            spSupplierTransaction.AmountDiscount = spSupplierTransaction.AmountDiscount * -1;
                            break;
                    }
                }
                else if (creditTrans.Action != "=" && debitTrans.Action == "=")
                {
                    spSupplierTransaction.TransactionType = FinancialTransactionTypeEnum.FinancialTransactionTypeCredit;
                    switch (creditTrans.Action)
                    {
                        case "+":

                            break;
                        case "-":
                            spSupplierTransaction.AmountNet = spSupplierTransaction.AmountNet * -1;
                            spSupplierTransaction.AmountFpa = spSupplierTransaction.AmountFpa * -1;
                            spSupplierTransaction.AmountDiscount = spSupplierTransaction.AmountDiscount * -1;
                            break;
                    }
                }

                transToAttach.SectionId = section.Id;
                transToAttach.FiscalPeriodId = 1;
                transToAttach.MaterialDocTypeId = docSeries.BuyMaterialDocTypeDefId;
                _context.BuyMaterialsDocuments.Add(transToAttach);

                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    throw;
                }

                spSupplierTransaction.CreatorId = _context.Entry(transToAttach).Entity.Id;

                _context.SupplierTransactions.Add(spSupplierTransaction);

                try
                {
                    await _context.SaveChangesAsync();
                    //throw (new Exception("Test Exception"));

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    throw;
                }


                foreach (var dataBuyDocLine in data.BuyDocLines)
                {
                    var materialId = dataBuyDocLine.MaterialId;
                    var material = await _context.Materials.SingleOrDefaultAsync(p => p.Id == materialId);
                    if (material is null)
                    {
                        //Handle error
                        ModelState.AddModelError(string.Empty, "Doc Line error null Material");
                        return NotFound(new
                        {
                            error = "Could not locate material in Doc Line "
                        });
                    }
                    #region MaterialLine
                    var buyMaterialLine = new BuyMaterialsDocLine();
                    buyMaterialLine.AmountFpa = 0;
                    buyMaterialLine.AmountNet = dataBuyDocLine.Price;
                    buyMaterialLine.DiscountRate = dataBuyDocLine.DiscountRate;
                    buyMaterialLine.MaterialId = dataBuyDocLine.MaterialId;
                    buyMaterialLine.Quontity1 = dataBuyDocLine.Q1;
                    buyMaterialLine.Quontity2 = dataBuyDocLine.Q2;
                    buyMaterialLine.PrimaryUnitId = dataBuyDocLine.MainMeasureUnitId;
                    buyMaterialLine.SecondaryUnitId = dataBuyDocLine.SecondaryMeasureUnitId;
                    buyMaterialLine.BuyDocumentId = transToAttach.Id;
                    buyMaterialLine.Etiology = transToAttach.Etiology;
                    buyMaterialLine.FpaRate = dataBuyDocLine.FpaRate;
                    //_context.Entry(transToAttach).Entity
                    transToAttach.BuyDocLines.Add(buyMaterialLine);
                    #endregion

                    #region Warehouse transaction



                    var buyMaterialTrans = new WarehouseTransaction();
                    buyMaterialTrans.AmountDiscount =
                        dataBuyDocLine.Price * (decimal)dataBuyDocLine.Q1 * (decimal)dataBuyDocLine.DiscountRate;
                    buyMaterialTrans.AmountNet = dataBuyDocLine.Price * (decimal)dataBuyDocLine.Q1 -
                                                 buyMaterialTrans.AmountDiscount;
                    buyMaterialTrans.AmountFpa = buyMaterialTrans.AmountNet * (decimal)dataBuyDocLine.FpaRate;
                    buyMaterialTrans.CompanyId = transToAttach.CompanyId;
                    buyMaterialTrans.Etiology = transToAttach.Etiology;
                    buyMaterialTrans.FiscalPeriodId = transToAttach.FiscalPeriodId;
                    buyMaterialTrans.FpaRate = dataBuyDocLine.FpaRate;
                    buyMaterialTrans.MaterialId = materialId;
                    buyMaterialTrans.PrimaryUnitId = dataBuyDocLine.MainMeasureUnitId;
                    buyMaterialTrans.SecondaryUnitId = dataBuyDocLine.SecondaryMeasureUnitId;
                    buyMaterialTrans.SectionId = section.Id;
                    buyMaterialTrans.CreatorId = transToAttach.Id;
                    buyMaterialTrans.TransDate = transToAttach.TransDate;
                    buyMaterialTrans.TransRefCode = transToAttach.TransRefCode;
                    var transWarehouseDef = docTypeDef.TransWarehouseDef;
                    await _context.Entry(transWarehouseDef)
                        .Reference(t => t.AmtImportsTrans)
                        .LoadAsync();
                    await _context.Entry(transWarehouseDef)
                        .Reference(t => t.AmtExportsTrans)
                        .LoadAsync();
                    await _context.Entry(transWarehouseDef)
                        .Reference(t => t.TransWarehouseDefaultDocSeriesDef)
                        .LoadAsync();
                    #endregion
                }

                try
                {
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return Ok(new { });


        }


       

        private void GetAmounts(FinancialMovement debitAction, FinancialMovement creditAction, decimal netAmount = 0,
            decimal vatAmount = 0, decimal discountAmount = 0, double q1 = 0, double q2 = 0)
        {
            if (debitAction == null) throw new ArgumentNullException(nameof(debitAction));
            if (creditAction == null) throw new ArgumentNullException(nameof(creditAction));

            var o = new ActionProduct();


            if (creditAction.Action == "=" && debitAction.Action != "=")
            {
                o.FinancialTransactionType = FinancialTransactionTypeEnum.FinancialTransactionTypeDebit;
                o.WarehouseTransactionTypeCode = WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                switch (debitAction.Action)
                {
                    case "+":
                        o.NetAmount = netAmount;
                        o.VatAmount = vatAmount;
                        o.DiscountAmount = discountAmount;
                        o.Quontity1 = q1;
                        o.Quontity2 = q2;
                        break;
                    case "-":
                        o.NetAmount = netAmount * -1;
                        o.VatAmount = vatAmount * -1;
                        o.DiscountAmount = discountAmount * -1;
                        o.Quontity1 = q1 * -1;
                        o.Quontity2 = q2 * -1;
                        break;
                }
            }
            else if (creditAction.Action != "=" && debitAction.Action == "=")
            {
                o.FinancialTransactionType = FinancialTransactionTypeEnum.FinancialTransactionTypeCredit;
                o.WarehouseTransactionTypeCode = WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                switch (creditAction.Action)
                {
                    case "+":
                        o.NetAmount = netAmount;
                        o.VatAmount = vatAmount;
                        o.DiscountAmount = discountAmount;
                        o.Quontity1 = q1;
                        o.Quontity2 = q2;
                        break;
                    case "-":
                        o.NetAmount = netAmount * -1;
                        o.VatAmount = vatAmount * -1;
                        o.DiscountAmount = discountAmount * -1;
                        o.Quontity1 = q1 * -1;
                        o.Quontity2 = q2 * -1;
                        break;
                }
            }
        }
    }
}