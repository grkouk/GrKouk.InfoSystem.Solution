using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyMaterialsDocs;
using GrKouk.WebApi.Data;
using static GrKouk.InfoSystem.Domain.FinConfig.FinancialTransactionTypeEnum;

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
            //TODO: Να βρίσκει τιμές μόνο για κινήσεις αγοράς ισως LasrPriceImport LastPriceExport???
            var lastPr = await _context.WarehouseTransactions.Where(m => m.Id == materialId)
                .Select(k => new
                {
                    LastPrice = k.UnitPrice
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
                return NotFound(new
                {
                    error = "Material not found "
                });
            }
            //Thread.Sleep(10000);
            return Ok(materialData);
        }
        [HttpGet("FiscalPeriod")]
        public async Task<IActionResult> GetFiscalPeriod(DateTime forDate)
        {
            Debug.Print("******Inside GetFiscal period " + forDate.ToString());
            var dateOfTrans = forDate;
            var fiscalPeriod = await _context.FiscalPeriods.FirstOrDefaultAsync(p =>
                dateOfTrans >= p.StartDate && dateOfTrans <= p.EndDate);
            if (fiscalPeriod == null)
            {
                Debug.Print("******Inside GetFiscal period No Fiscal Period found" );
                ModelState.AddModelError(string.Empty, "No Fiscal Period covers Transaction Date");
                return NotFound(new
                {
                    error = "No fiscal period includes date"
                });

            }
            Debug.Print("******Inside GetFiscal period Returning period id " + fiscalPeriod.Id);
            return Ok(new {PeriodId = fiscalPeriod.Id});
        }

        [HttpPost("MaterialBuyDoc")]
        public async Task<IActionResult> PostMaterialBuyDoc([FromBody] BuyMaterialsDocCreateAjaxDto data)
        {
            const string sectionCode = "SYS-BUY-MATERIALS-SCN";

            var transToAttachNoLines = _mapper.Map<BuyMaterialsDocCreateAjaxNoLinesDto>(data);
            var transToAttach = _mapper.Map<BuyMaterialsDocument>(transToAttachNoLines);
            var dateOfTrans = data.TransDate;
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
                #region Fiscal Period

                var fiscalPeriod = await _context.FiscalPeriods.FirstOrDefaultAsync(p =>
                    p.StartDate.CompareTo(dateOfTrans) > 0 & p.EndDate.CompareTo(dateOfTrans) < 0);
                if (fiscalPeriod==null)
                {
                    transaction.Rollback();
                    ModelState.AddModelError(string.Empty, "No Fiscal Period covers Transaction Date");
                    return NotFound(new
                    {
                        error = "No Fiscal Period covers Transaction Date"
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
                await _context.Entry(docTypeDef).Reference(t => t.TransWarehouseDef)
                    .LoadAsync();

                var transSupplierDef = docTypeDef.TransSupplierDef;
                var transWarehouseDef = docTypeDef.TransWarehouseDef;
                await _context.Entry(transWarehouseDef)
                    .Reference(t => t.TransWarehouseDefaultDocSeriesDef)
                    .LoadAsync();
                await _context.Entry(transSupplierDef)
                    .Reference(t => t.TransSupplierDefaultDocSeries)
                    .LoadAsync();

                var transSupDefaultSeries = transSupplierDef.TransSupplierDefaultDocSeries;


                var spSupplierTransaction = _mapper.Map<SupplierTransaction>(data);
                spSupplierTransaction.SectionId = section.Id;
                spSupplierTransaction.TransSupplierDocTypeId = transSupDefaultSeries.TransSupplierDocTypeDefId;
                spSupplierTransaction.TransSupplierDocSeriesId = transSupDefaultSeries.Id;
                spSupplierTransaction.FiscalPeriodId = fiscalPeriod.Id;
                switch (transSupplierDef.FinancialTransType)
                {
                    case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeNoChange:
                        break;
                    case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeDebit:
                        spSupplierTransaction.FinancialAction = FinancialTransTypeEnum.FinancialTransTypeDebit;
                        spSupplierTransaction.TransactionType = FinancialTransactionTypeDebit;
                        break;
                    case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeCredit:
                        spSupplierTransaction.FinancialAction = FinancialTransTypeEnum.FinancialTransTypeCredit;
                        spSupplierTransaction.TransactionType = FinancialTransactionTypeCredit;
                        break;
                    case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeNegativeDebit:
                        spSupplierTransaction.FinancialAction = FinancialTransTypeEnum.FinancialTransTypeNegativeDebit;
                        spSupplierTransaction.TransactionType = FinancialTransactionTypeDebit;
                        spSupplierTransaction.AmountNet = spSupplierTransaction.AmountNet * -1;
                        spSupplierTransaction.AmountFpa = spSupplierTransaction.AmountFpa * -1;
                        spSupplierTransaction.AmountDiscount = spSupplierTransaction.AmountDiscount * -1;
                        break;
                    case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeNegativeCredit:
                        spSupplierTransaction.FinancialAction = FinancialTransTypeEnum.FinancialTransTypeNegativeCredit;
                        spSupplierTransaction.TransactionType = FinancialTransactionTypeCredit;
                        spSupplierTransaction.AmountNet = spSupplierTransaction.AmountNet * -1;
                        spSupplierTransaction.AmountFpa = spSupplierTransaction.AmountFpa * -1;
                        spSupplierTransaction.AmountDiscount = spSupplierTransaction.AmountDiscount * -1;
                        break;
                    default:
                        break;
                }

                transToAttach.SectionId = section.Id;
                transToAttach.FiscalPeriodId = fiscalPeriod.Id;
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
                    decimal unitPrice = dataBuyDocLine.Price;
                    decimal units = (decimal)dataBuyDocLine.Q1;
                    decimal fpaRate = (decimal)dataBuyDocLine.FpaRate;
                    decimal discountRate = (decimal)dataBuyDocLine.DiscountRate;
                    decimal lineNetAmount = unitPrice * units;
                    decimal lineDiscountAmount = lineNetAmount * discountRate;
                    decimal lineFpaAmount = (lineNetAmount - lineDiscountAmount) * fpaRate;
                    buyMaterialLine.UnitPrice = unitPrice;
                    buyMaterialLine.AmountFpa = lineFpaAmount;
                    buyMaterialLine.AmountNet = lineNetAmount;
                    buyMaterialLine.AmountDiscount = lineDiscountAmount;
                    buyMaterialLine.DiscountRate = discountRate;
                    buyMaterialLine.FpaRate = fpaRate;
                    buyMaterialLine.MaterialId = dataBuyDocLine.MaterialId;
                    buyMaterialLine.Quontity1 = dataBuyDocLine.Q1;
                    buyMaterialLine.Quontity2 = dataBuyDocLine.Q2;
                    buyMaterialLine.PrimaryUnitId = dataBuyDocLine.MainUnitId;
                    buyMaterialLine.SecondaryUnitId = dataBuyDocLine.SecUnitId;
                    buyMaterialLine.BuyDocumentId = transToAttach.Id;
                    buyMaterialLine.Etiology = transToAttach.Etiology;
                    //_context.Entry(transToAttach).Entity
                    transToAttach.BuyDocLines.Add(buyMaterialLine);
                    #endregion

                    #region Warehouse transaction
                    var warehouseTrans = new WarehouseTransaction();
                    warehouseTrans.FpaRate = fpaRate;
                    warehouseTrans.DiscountRate = discountRate;
                    warehouseTrans.UnitPrice = unitPrice;
                    warehouseTrans.AmountDiscount = lineDiscountAmount;
                    warehouseTrans.AmountNet = lineNetAmount;
                    warehouseTrans.AmountFpa = lineFpaAmount;
                    warehouseTrans.CompanyId = transToAttach.CompanyId;
                    warehouseTrans.Etiology = transToAttach.Etiology;
                    warehouseTrans.FiscalPeriodId = transToAttach.FiscalPeriodId;

                    warehouseTrans.MaterialId = materialId;
                    warehouseTrans.PrimaryUnitId = dataBuyDocLine.MainUnitId;
                    warehouseTrans.SecondaryUnitId = dataBuyDocLine.SecUnitId;
                    warehouseTrans.SectionId = section.Id;
                    warehouseTrans.CreatorId = transToAttach.Id;
                    warehouseTrans.TransDate = transToAttach.TransDate;
                    warehouseTrans.TransRefCode = transToAttach.TransRefCode;
                    warehouseTrans.UnitFactor = (decimal)dataBuyDocLine.Factor;

                    warehouseTrans.TransWarehouseDocSeriesId = transWarehouseDef.TransWarehouseDefaultDocSeriesDefId;
                    warehouseTrans.TransWarehouseDocTypeId = transWarehouseDef.TransWarehouseDefaultDocSeriesDef
                        .TransWarehouseDocTypeDefId;
                    warehouseTrans.InventoryAction = transWarehouseDef.InventoryTransType;
                    switch (transWarehouseDef.InventoryTransType)
                    {
                        case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumNoChange:
                            break;
                        case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumImport:

                            warehouseTrans.TransactionType =
                                WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                            warehouseTrans.Quontity1 = dataBuyDocLine.Q1;
                            warehouseTrans.Quontity2 = dataBuyDocLine.Q2;
                            break;
                        case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumExport:

                            warehouseTrans.TransactionType =
                                WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                            warehouseTrans.Quontity1 = dataBuyDocLine.Q1;
                            warehouseTrans.Quontity2 = dataBuyDocLine.Q2;
                            break;
                        case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumNegativeImport:

                            warehouseTrans.TransactionType =
                                WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                            warehouseTrans.Quontity1 = dataBuyDocLine.Q1 * -1;
                            warehouseTrans.Quontity2 = dataBuyDocLine.Q2 * -1;
                            break;
                        case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumNegativeExport:

                            warehouseTrans.TransactionType =
                                WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                            warehouseTrans.Quontity1 = dataBuyDocLine.Q1 * -1;
                            warehouseTrans.Quontity2 = dataBuyDocLine.Q2 * -1;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    warehouseTrans.InventoryValueAction = transWarehouseDef.InventoryValueTransType;

                    switch (transWarehouseDef.InventoryValueTransType)
                    {
                        case WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumNoChange:
                            break;
                        case WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumIncrease:

                            break;
                        case WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumDecrease:

                            break;
                        case WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumNegativeIncrease:
                            warehouseTrans.AmountNet = warehouseTrans.AmountNet * -1;
                            warehouseTrans.AmountDiscount = warehouseTrans.AmountDiscount * -1;
                            warehouseTrans.AmountFpa = warehouseTrans.AmountFpa * -1;
                            break;
                        case WarehouseValueTransTypeEnum.WarehouseValueTransTypeEnumNegativeDecrease:
                            warehouseTrans.AmountNet = warehouseTrans.AmountNet * -1;
                            warehouseTrans.AmountDiscount = warehouseTrans.AmountDiscount * -1;
                            warehouseTrans.AmountFpa = warehouseTrans.AmountFpa * -1;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    _context.WarehouseTransactions.Add(warehouseTrans);

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