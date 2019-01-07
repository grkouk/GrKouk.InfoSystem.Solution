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
using GrKouk.InfoSystem.Dtos.WebDtos.SupplierTransactions;
using GrKouk.WebApi.Data;
using Remotion.Linq.Clauses;
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
        [HttpGet("WarehouseTransType")]
        public async Task<IActionResult> GetWarehouseTransType(int seriesId)
        {
            Debug.Print("******Inside GetWarehouseTransType for series ID " + seriesId.ToString());
            
            var transWarehouseDocSeriesDef = await _context.TransWarehouseDocSeriesDefs.FirstOrDefaultAsync(p =>
                p.Id==seriesId);
            if (transWarehouseDocSeriesDef == null)
            {
                Debug.Print("******Inside GetWarehouseTransType No Series found");
                ModelState.AddModelError(string.Empty, "No Series found");
                return NotFound(new
                {
                    error = "No Series found"
                });

            }
            await _context.Entry(transWarehouseDocSeriesDef).Reference(t => t.TransWarehouseDocTypeDef).LoadAsync();
            var transWarehouseDocTypeDef = transWarehouseDocSeriesDef.TransWarehouseDocTypeDef;
            await _context.Entry(transWarehouseDocTypeDef).Reference(t => t.TransWarehouseDef).LoadAsync();
            var transWarehouseDef = transWarehouseDocTypeDef.TransWarehouseDef;
            var inventoryActionType = transWarehouseDef.InventoryTransType;
            string transType="";
            switch (inventoryActionType)
            {
                case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumNoChange:
                    transType = "WarehouseTransactionTypeIgnore";
                    break;
                case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumImport:
                    transType= "WarehouseTransactionTypeImport";
                    break;
                case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumExport:
                    transType = "WarehouseTransactionTypeExport";
                    break;
                case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumNegativeImport:
                    transType = "WarehouseTransactionTypeImport";
                    break;
                case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumNegativeExport:
                    transType = "WarehouseTransactionTypeExport";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Debug.Print("******Inside GetWarehouseTransType Returning Action value " + transType);
            return Ok(new { TransType = transType });
        }
        [HttpPost("MaterialBuyDoc")]
        public async Task<IActionResult> PostMaterialBuyDoc([FromBody] BuyMaterialsDocCreateAjaxDto data)
        {
            const string sectionCode = "SYS-BUY-MATERIALS-SCN";
            bool noSupplierTrans = false;
            bool noWarehouseTrans = false;
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
                    dateOfTrans >= p.StartDate && dateOfTrans <= p.EndDate);
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
                var docId = _context.Entry(transToAttach).Entity.Id;

                if (transSupplierDef.TransSupplierDefaultDocSeriesId > 0)
                {
                   var  transSupDefaultSeries = await
                        _context.TransSupplierDocSeriesDefs.FirstOrDefaultAsync(p =>
                            p.Id == transSupplierDef.TransSupplierDefaultDocSeriesId);
                    if (transSupDefaultSeries == null)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Default series for supplier transaction not found");
                        return NotFound(new
                        {
                            error = "Default series for supplier transaction not found"
                        });
                    }
                    var spSupplierTransaction = _mapper.Map<SupplierTransaction>(data);
                    spSupplierTransaction.SectionId = section.Id;
                    spSupplierTransaction.TransSupplierDocTypeId = transSupDefaultSeries.TransSupplierDocTypeDefId;
                    spSupplierTransaction.TransSupplierDocSeriesId = transSupDefaultSeries.Id;
                    spSupplierTransaction.FiscalPeriodId = fiscalPeriod.Id;
                    spSupplierTransaction.CreatorId = docId;

                    switch (transSupplierDef.FinancialTransType)
                    {
                        case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeNoChange:
                            spSupplierTransaction.FinancialAction = FinancialTransTypeEnum.FinancialTransTypeNoChange;
                            spSupplierTransaction.TransactionType = FinancialTransactionTypeIgnore;
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
                    _context.SupplierTransactions.Add(spSupplierTransaction);
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
                }

                int warehouseSeriesId=0;
                int warehouseTypeId=0;

                if (transWarehouseDef.TransWarehouseDefaultDocSeriesDefId > 0)
                {
                    
                    var transWarehouseDefaultSeries =
                        await _context.TransWarehouseDocSeriesDefs.FirstOrDefaultAsync(p =>
                            p.Id == transWarehouseDef.TransWarehouseDefaultDocSeriesDefId);
                    if (transWarehouseDefaultSeries == null)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Default series for warehouse transaction not found");
                        return NotFound(new
                        {
                            error = "Default series for warehouse transaction not found"
                        });
                    }
                    noWarehouseTrans = false;
                    warehouseSeriesId = transWarehouseDef.TransWarehouseDefaultDocSeriesDefId;
                    warehouseTypeId = transWarehouseDefaultSeries.TransWarehouseDocTypeDefId;
                }
                else
                {
                    noWarehouseTrans = true;
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
                    buyMaterialLine.Factor = dataBuyDocLine.Factor;
                    buyMaterialLine.BuyDocumentId = docId;
                    buyMaterialLine.Etiology = transToAttach.Etiology;
                    //_context.Entry(transToAttach).Entity
                    transToAttach.BuyDocLines.Add(buyMaterialLine);
                    #endregion

                    if (!noWarehouseTrans)
                    {
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

                        warehouseTrans.TransWarehouseDocSeriesId = warehouseSeriesId;
                        warehouseTrans.TransWarehouseDocTypeId = warehouseTypeId;
                        warehouseTrans.InventoryAction = transWarehouseDef.InventoryTransType;
                        switch (transWarehouseDef.InventoryTransType)
                        {
                            case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumNoChange:
                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeIgnore;
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

        [HttpPut("MaterialBuyDocUpdate")]
        public async Task<IActionResult> PutMaterialBuyDoc([FromBody] BuyMaterialsDocModifyAjaxDto data)
        {
            const string sectionCode = "SYS-BUY-MATERIALS-SCN";
            bool noWarehouseTrans;
          
            var transToAttachNoLines = _mapper.Map<BuyMaterialsDocModifyAjaxNoLinesDto>(data);
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
                _context.BuyMaterialsDocLines.RemoveRange(_context.BuyMaterialsDocLines.Where(p=>p.BuyDocumentId==data.Id));
                _context.SupplierTransactions.RemoveRange(_context.SupplierTransactions.Where(p=>p.SectionId==section.Id && p.CreatorId==data.Id));
                _context.WarehouseTransactions.RemoveRange(_context.WarehouseTransactions.Where(p => p.SectionId == section.Id && p.CreatorId == data.Id));
                #region Fiscal Period

                var fiscalPeriod = await _context.FiscalPeriods.FirstOrDefaultAsync(p =>
                    dateOfTrans >= p.StartDate && dateOfTrans <= p.EndDate);
                if (fiscalPeriod == null)
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
                transToAttach.SectionId = section.Id;
                transToAttach.FiscalPeriodId = fiscalPeriod.Id;
                transToAttach.MaterialDocTypeId = docSeries.BuyMaterialDocTypeDefId;

                _context.Entry(transToAttach).State = EntityState.Modified;
                var docId = transToAttach.Id;

                //--------------------------------------
                if (transSupplierDef.TransSupplierDefaultDocSeriesId > 0)
                {
                    var transSupDefaultSeries = await
                         _context.TransSupplierDocSeriesDefs.FirstOrDefaultAsync(p =>
                             p.Id == transSupplierDef.TransSupplierDefaultDocSeriesId);
                    if (transSupDefaultSeries == null)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Default series for supplier transaction not found");
                        return NotFound(new
                        {
                            error = "Default series for supplier transaction not found"
                        });
                    }
                    var spSupplierCreateDto = _mapper.Map<SupplierTransactionCreateDto>(data);
                    //Ετσι δεν μεταφέρει το Id απο το data
                    var spSupplierTransaction = _mapper.Map<SupplierTransaction>(spSupplierCreateDto);
                   
                    spSupplierTransaction.SectionId = section.Id;
                    spSupplierTransaction.TransSupplierDocTypeId = transSupDefaultSeries.TransSupplierDocTypeDefId;
                    spSupplierTransaction.TransSupplierDocSeriesId = transSupDefaultSeries.Id;
                    spSupplierTransaction.FiscalPeriodId = fiscalPeriod.Id;
                    spSupplierTransaction.CreatorId = docId;

                    switch (transSupplierDef.FinancialTransType)
                    {
                        case InfoSystem.Domain.FinConfig.FinancialTransTypeEnum.FinancialTransTypeNoChange:
                            spSupplierTransaction.FinancialAction = FinancialTransTypeEnum.FinancialTransTypeNoChange;
                            spSupplierTransaction.TransactionType = FinancialTransactionTypeIgnore;
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
                    _context.SupplierTransactions.Add(spSupplierTransaction);
                    //try
                    //{
                    //    await _context.SaveChangesAsync();

                    //}
                    //catch (Exception e)
                    //{
                    //    Console.WriteLine(e);
                    //    transaction.Rollback();
                    //    throw;
                    //}
                }
                //-------------------------------------

                int warehouseSeriesId = 0;
                int warehouseTypeId = 0;

                if (transWarehouseDef.TransWarehouseDefaultDocSeriesDefId > 0)
                {

                    var transWarehouseDefaultSeries =
                        await _context.TransWarehouseDocSeriesDefs.FirstOrDefaultAsync(p =>
                            p.Id == transWarehouseDef.TransWarehouseDefaultDocSeriesDefId);
                    if (transWarehouseDefaultSeries == null)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Default series for warehouse transaction not found");
                        return NotFound(new
                        {
                            error = "Default series for warehouse transaction not found"
                        });
                    }
                    noWarehouseTrans = false;
                    warehouseSeriesId = transWarehouseDef.TransWarehouseDefaultDocSeriesDefId;
                    warehouseTypeId = transWarehouseDefaultSeries.TransWarehouseDocTypeDefId;
                }
                else
                {
                    noWarehouseTrans = true;
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
                    buyMaterialLine.Factor = dataBuyDocLine.Factor;
                    buyMaterialLine.BuyDocumentId = docId;
                    buyMaterialLine.Etiology = transToAttach.Etiology;
                    //_context.Entry(transToAttach).Entity
                    transToAttach.BuyDocLines.Add(buyMaterialLine);
                    #endregion

                    if (!noWarehouseTrans)
                    {
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

                        warehouseTrans.TransWarehouseDocSeriesId = warehouseSeriesId;
                        warehouseTrans.TransWarehouseDocTypeId = warehouseTypeId;
                        warehouseTrans.InventoryAction = transWarehouseDef.InventoryTransType;
                        switch (transWarehouseDef.InventoryTransType)
                        {
                            case WarehouseInventoryTransTypeEnum.WarehouseInventoryTransTypeEnumNoChange:
                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeIgnore;
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
    }
}