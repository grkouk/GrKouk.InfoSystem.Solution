using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GrKouk.InfoSystem.Domain.FinConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.Materials;
using GrKouk.InfoSystem.Dtos.WebDtos.SellDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.TransactorTransactions;
using GrKouk.WebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using static GrKouk.InfoSystem.Domain.FinConfig.FinancialTransactionTypeEnum;

namespace GrKouk.WebRazor.Controllers
{
    [Authorize]
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
        [HttpGet("SetCompanyInSession")]
        public IActionResult CompanyInSession(string companyId)
        {
            HttpContext.Session.SetString ("CompanyId",companyId);
            return Ok(new {});
        }

        [HttpGet("SeekBarcode")]
        public async Task<IActionResult> GetMaterialFromBarcode(string barcode)
        {
            //var sessionCompanyId = HttpContext.Session.GetString("CompanyId");
            var materials = await _context.MaterialCodes
                .Include(p => p.Material)
                .ThenInclude(p=>p.FpaDef)
                .Where(p => p.Code == barcode && p.CodeType == MaterialCodeTypeEnum.CodeTypeEnumBarcode)
                .ToListAsync();

                //.ProjectTo<MaterialSearchListDto>(_mapper.ConfigurationProvider)
                //.Select(p => new { label = p.Label, value = p.Id }).ToListAsync();

            if (materials == null)
            {
                return NotFound();
            }

            if (materials.Count>1)
            {
                return NotFound();
            }

            var material = materials[0].Material;
            var usedUnit = materials[0].CodeUsedUnit;
            double unitFactor;
            string unitToUse;
            switch (usedUnit)
            {
                case MaterialCodeUsedUnitEnum.CodeUsedUnitEnumMain:
                    unitFactor = 1;
                    unitToUse = "MAIN";
                    break;
                case MaterialCodeUsedUnitEnum.CodeUsedUnitEnumSecondary:
                    unitFactor = material.SecondaryUnitToMainRate;
                    unitToUse = "SEC";
                    break;
                case MaterialCodeUsedUnitEnum.CodeUsedUnitEnumBuy:
                    unitFactor = material.BuyUnitToMainRate;
                    unitToUse = "BUY";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var lastPr = await _context.WarehouseTransactions.Where(m => m.Id == material.Id)
                .Select(k => new
                {
                    LastPrice = k.UnitPrice
                }).FirstOrDefaultAsync();

            var lastPrice = lastPr?.LastPrice ?? 0;

            return Ok(new
            {
                Id = material.Id,
                Name =material.Name,
                fpaId = material.FpaDefId,
                lastPrice = lastPrice,
                fpaRate = material.FpaDef.Rate,
                unitToUse=unitToUse,
                Factor=unitFactor

            });
        }
        [HttpGet("SearchForMaterials")]
        public async Task<IActionResult> GetMaterials(string term)
        {
            var sessionCompanyId = HttpContext.Session.GetString("CompanyId");
            var materials = await _context.Materials.Where(p => p.Name.Contains(term) )
                .ProjectTo<MaterialSearchListDto>(_mapper.ConfigurationProvider)
                .Select(p => new { label = p.Label, value = p.Id }).ToListAsync();

            if (materials == null)
            {
                return NotFound();
            }

            return Ok(materials);
        }
        [HttpGet("SearchForServices")]
        public async Task<IActionResult> GetServices(string term)
        {

            var materials = await _context.Materials.Where(p => p.Name.Contains(term) && p.MaterialNature == MaterialNatureEnum.MaterialNatureEnumService)
                .Select(p => new { label = p.Name, value = p.Id }).ToListAsync();

            if (materials == null)
            {
                return NotFound();
            }

            return Ok(materials);
        }
        [HttpGet("SearchForExpenses")]
        public async Task<IActionResult> GetExpenses(string term)
        {

            var materials = await _context.Materials.Where(p => p.Name.Contains(term) && p.MaterialNature == MaterialNatureEnum.MaterialNatureEnumExpense)
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
            var lastPr = await _context.WarehouseTransactions.OrderByDescending(p=>p.TransDate).Where(m => m.MaterialId == materialId)
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
                    priceNetto=p.PriceNetto,
                    priceBrutto=p.PriceBrutto,

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

        [HttpGet("SalesSeriesData")]
        public async Task<IActionResult> GetSalesSeriesData(int seriesId)
        {
            Debug.Print("Inside GetSalesSeriesData " + seriesId.ToString());
            var salesSeriesDef = await _context.SellDocSeriesDefs.SingleOrDefaultAsync(p =>p.Id==seriesId);
            if (salesSeriesDef == null)
            {
                Debug.Print("Inside GetSalesSeriesData No Series found");
                return NotFound(new
                {
                    error = "No Series Found"
                });
            }

            await _context.Entry(salesSeriesDef)
                .Reference(p => p.SellDocTypeDef)
                .LoadAsync();
            var salesTypeDef = salesSeriesDef.SellDocTypeDef;
            var usedPrice = salesTypeDef.UsedPrice;

            Debug.Print("Inside GetSalesSeriesData Returning usedPrice " + usedPrice.ToString());
            return Ok(new { UsedPrice = usedPrice });
        }
        [HttpGet("BuySeriesData")]
        public async Task<IActionResult> GetBuySeriesData(int seriesId)
        {
            Debug.Print("Inside GetBuySeriesData " + seriesId.ToString());
            var buySeriesDef = await _context.BuyDocSeriesDefs.SingleOrDefaultAsync(p => p.Id == seriesId);
            if (buySeriesDef == null)
            {
                Debug.Print("Inside GetBuySeriesData No Series found");
                return NotFound(new
                {
                    error = "No Series Found"
                });
            }

            await _context.Entry(buySeriesDef)
                .Reference(p => p.BuyDocTypeDef)
                .LoadAsync();
            var buyTypeDef = buySeriesDef.BuyDocTypeDef;
            var usedPrice = buyTypeDef.UsedPrice;

            Debug.Print("Inside GetBuySeriesData Returning usedPrice " + usedPrice.ToString());
            return Ok(new { UsedPrice = usedPrice });
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
            var inventoryActionType = transWarehouseDef.MaterialInventoryAction;
            string transType="";
            switch (inventoryActionType)
            {
                case InventoryActionEnum.InventoryActionEnumNoChange:
                    transType = "WarehouseTransactionTypeIgnore";
                    break;
                case InventoryActionEnum.InventoryActionEnumImport:
                    transType= "WarehouseTransactionTypeImport";
                    break;
                case InventoryActionEnum.InventoryActionEnumExport:
                    transType = "WarehouseTransactionTypeExport";
                    break;
                case InventoryActionEnum.InventoryActionEnumNegativeImport:
                    transType = "WarehouseTransactionTypeImport";
                    break;
                case InventoryActionEnum.InventoryActionEnumNegativeExport:
                    transType = "WarehouseTransactionTypeExport";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Debug.Print("******Inside GetWarehouseTransType Returning Action value " + transType);
            return Ok(new { TransType = transType });
        }
        [HttpPost("MaterialBuyDoc")]
        public async Task<IActionResult> PostMaterialBuyDoc([FromBody] BuyDocCreateAjaxDto data)
        {
            const string sectionCode = "SYS-BUY-MATERIALS-SCN";
            bool noSupplierTrans = false;
            bool noWarehouseTrans = false;
            var transToAttachNoLines = _mapper.Map<BuyDocCreateAjaxNoLinesDto>(data);
            var transToAttach = _mapper.Map<BuyDocument>(transToAttachNoLines);
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
                    _context.BuyDocSeriesDefs.SingleOrDefaultAsync(m => m.Id == data.BuyDocSeriesId);

                if (docSeries is null)
                {
                    transaction.Rollback();
                    ModelState.AddModelError(string.Empty, "Δεν βρέθηκε η σειρά παραστατικού");
                    return NotFound(new
                    {
                        error = "Buy Doc Series not found"
                    });
                }

                await _context.Entry(docSeries).Reference(t => t.BuyDocTypeDef).LoadAsync();
                var docTypeDef = docSeries.BuyDocTypeDef;
                //await _context.Entry(docTypeDef)
                //      .Reference(t => t.TransSupplierDef)
                //      .LoadAsync();
                await _context.Entry(docTypeDef)
                    .Reference(t => t.TransTransactorDef)
                    .LoadAsync();

                await _context.Entry(docTypeDef).Reference(t => t.TransWarehouseDef)
                    .LoadAsync();

                //var transSupplierDef = docTypeDef.TransSupplierDef;
                var transTransactorDef = docTypeDef.TransTransactorDef;
                var transWarehouseDef = docTypeDef.TransWarehouseDef;

                transToAttach.SectionId = section.Id;
                transToAttach.FiscalPeriodId = fiscalPeriod.Id;
                transToAttach.BuyDocTypeId = docSeries.BuyDocTypeDefId;
                _context.BuyDocuments.Add(transToAttach);

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

                //if (transSupplierDef.DefaultDocSeriesId > 0)
                //{
                //   var  transSupDefaultSeries = await
                //        _context.TransSupplierDocSeriesDefs.FirstOrDefaultAsync(p =>
                //            p.Id == transSupplierDef.DefaultDocSeriesId);
                //    if (transSupDefaultSeries == null)
                //    {
                //        transaction.Rollback();
                //        ModelState.AddModelError(string.Empty, "Default series for supplier transaction not found");
                //        return NotFound(new
                //        {
                //            error = "Default series for supplier transaction not found"
                //        });
                //    }
                //    var spSupplierTransaction = _mapper.Map<SupplierTransaction>(data);
                //    spSupplierTransaction.SectionId = section.Id;
                //    spSupplierTransaction.TransSupplierDocTypeId = transSupDefaultSeries.TransSupplierDocTypeDefId;
                //    spSupplierTransaction.TransSupplierDocSeriesId = transSupDefaultSeries.Id;
                //    spSupplierTransaction.FiscalPeriodId = fiscalPeriod.Id;
                //    spSupplierTransaction.CreatorId = docId;

                //    switch (transSupplierDef.FinancialAction)
                //    {
                //        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNoChange:
                //            spSupplierTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNoChange;
                //            spSupplierTransaction.TransactionType = FinancialTransactionTypeIgnore;
                //            break;
                //        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumDebit:
                //            spSupplierTransaction.FinancialAction = FinActionsEnum.FinActionsEnumDebit;
                //            spSupplierTransaction.TransactionType = FinancialTransactionTypeDebit;
                //            break;
                //        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumCredit:
                //            spSupplierTransaction.FinancialAction = FinActionsEnum.FinActionsEnumCredit;
                //            spSupplierTransaction.TransactionType = FinancialTransactionTypeCredit;
                //            break;
                //        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeDebit:
                //            spSupplierTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNegativeDebit;
                //            spSupplierTransaction.TransactionType = FinancialTransactionTypeDebit;
                //            spSupplierTransaction.AmountNet = spSupplierTransaction.AmountNet * -1;
                //            spSupplierTransaction.AmountFpa = spSupplierTransaction.AmountFpa * -1;
                //            spSupplierTransaction.AmountDiscount = spSupplierTransaction.AmountDiscount * -1;
                //            break;
                //        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeCredit:
                //            spSupplierTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNegativeCredit;
                //            spSupplierTransaction.TransactionType = FinancialTransactionTypeCredit;
                //            spSupplierTransaction.AmountNet = spSupplierTransaction.AmountNet * -1;
                //            spSupplierTransaction.AmountFpa = spSupplierTransaction.AmountFpa * -1;
                //            spSupplierTransaction.AmountDiscount = spSupplierTransaction.AmountDiscount * -1;
                //            break;
                //        default:
                //            break;
                //    }
                //    _context.SupplierTransactions.Add(spSupplierTransaction);
                //    try
                //    {
                //        await _context.SaveChangesAsync();

                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine(e);
                //        transaction.Rollback();
                //        throw;
                //    }
                //}
                if (transTransactorDef.DefaultDocSeriesId > 0)
                {
                    var transTransactorDefaultSeries = await
                         _context.TransTransactorDocSeriesDefs.FirstOrDefaultAsync(p =>
                             p.Id == transTransactorDef.DefaultDocSeriesId);
                    if (transTransactorDefaultSeries == null)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Default series for transactor transaction not found");
                        return NotFound(new
                        {
                            error = "Default series for transactor transaction not found"
                        });
                    }
                    var sTransactorTransaction = _mapper.Map<TransactorTransaction>(data);
                    sTransactorTransaction.TransactorId = data.TransactorId;
                    sTransactorTransaction.SectionId = section.Id;
                    sTransactorTransaction.TransTransactorDocTypeId = transTransactorDefaultSeries.TransTransactorDocTypeDefId;
                    sTransactorTransaction.TransTransactorDocSeriesId = transTransactorDefaultSeries.Id;
                    sTransactorTransaction.FiscalPeriodId = fiscalPeriod.Id;
                    sTransactorTransaction.CreatorId = docId;

                    switch (transTransactorDef.FinancialTransAction)
                    {
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNoChange:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNoChange;
                            sTransactorTransaction.TransDiscountAmount = 0;
                            sTransactorTransaction.TransFpaAmount = 0;
                            sTransactorTransaction.TransNetAmount = 0;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumDebit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumDebit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumCredit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumCredit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeDebit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNegativeDebit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount*-1;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa*-1;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet*-1;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeCredit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNegativeCredit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount * -1;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa * -1;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet * -1;
                            break;
                        default:
                            break;
                    }
                    _context.TransactorTransactions.Add(sTransactorTransaction);
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

                if (transWarehouseDef.DefaultDocSeriesId > 0)
                {
                    
                    var transWarehouseDefaultSeries =
                        await _context.TransWarehouseDocSeriesDefs.FirstOrDefaultAsync(p =>
                            p.Id == transWarehouseDef.DefaultDocSeriesId);
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
                    warehouseSeriesId = transWarehouseDef.DefaultDocSeriesId;
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
                    var buyMaterialLine = new BuyDocLine();
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

                        switch (material.MaterialNature)
                        {
                            case MaterialNatureEnum.MaterialNatureEnumUndefined:
                                throw new ArgumentOutOfRangeException();
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumMaterial:
                                warehouseTrans.InventoryAction = transWarehouseDef.MaterialInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.MaterialInventoryValueAction;

                                break;
                            case MaterialNatureEnum.MaterialNatureEnumService:
                                warehouseTrans.InventoryAction = transWarehouseDef.ServiceInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ServiceInventoryValueAction;
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumExpense:
                                warehouseTrans.InventoryAction = transWarehouseDef.ExpenseInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ExpenseInventoryValueAction;
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumIncome:
                                warehouseTrans.InventoryAction = transWarehouseDef.IncomeInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.IncomeInventoryValueAction;
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumFixedAsset:
                                warehouseTrans.InventoryAction = transWarehouseDef.FixedAssetInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.FixedAssetInventoryValueAction;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        
                        switch (warehouseTrans.InventoryAction)
                        {
                            case InventoryActionEnum.InventoryActionEnumNoChange:
                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeIgnore;
                                warehouseTrans.TransQ1 = 0;
                                warehouseTrans.TransQ2 = 0;
                                break;
                            case InventoryActionEnum.InventoryActionEnumImport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                                warehouseTrans.Quontity1 = dataBuyDocLine.Q1;
                                warehouseTrans.Quontity2 = dataBuyDocLine.Q2;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2; 
                                break;
                            case InventoryActionEnum.InventoryActionEnumExport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                                warehouseTrans.Quontity1 = dataBuyDocLine.Q1;
                                warehouseTrans.Quontity2 = dataBuyDocLine.Q2;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2;
                                break;
                            case InventoryActionEnum.InventoryActionEnumNegativeImport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                                warehouseTrans.Quontity1 = dataBuyDocLine.Q1;
                                warehouseTrans.Quontity2 = dataBuyDocLine.Q2 ;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1*-1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2*-1;
                                break;
                            case InventoryActionEnum.InventoryActionEnumNegativeExport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                                warehouseTrans.Quontity1 = dataBuyDocLine.Q1 ;
                                warehouseTrans.Quontity2 = dataBuyDocLine.Q2 ;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1 * -1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2 * -1;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }


                        switch (warehouseTrans.InventoryValueAction)
                        {
                            case InventoryValueActionEnum.InventoryValueActionEnumNoChange:
                                warehouseTrans.TransNetAmount = 0 ;
                                warehouseTrans.TransFpaAmount = 0;
                                warehouseTrans.TransDiscountAmount = 0;
                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumIncrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount;
                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumDecrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount;
                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumNegativeIncrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet*-1;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa*-1;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount*-1;
                               
                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumNegativeDecrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet * -1;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa * -1;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount * -1;
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
        public async Task<IActionResult> PutMaterialBuyDoc([FromBody] BuyDocModifyAjaxDto data)
        {
            const string sectionCode = "SYS-BUY-MATERIALS-SCN";
            bool noWarehouseTrans;
          
            var transToAttachNoLines = _mapper.Map<BuyDocModifyAjaxNoLinesDto>(data);
            var transToAttach = _mapper.Map<BuyDocument>(transToAttachNoLines);
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
                _context.BuyDocLines.RemoveRange(_context.BuyDocLines.Where(p=>p.BuyDocumentId==data.Id));
                _context.TransactorTransactions.RemoveRange(_context.TransactorTransactions.Where(p => p.SectionId == section.Id && p.CreatorId == data.Id));
                //_context.SupplierTransactions.RemoveRange(_context.SupplierTransactions.Where(p=>p.SectionId==section.Id && p.CreatorId==data.Id));
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
                    _context.BuyDocSeriesDefs.SingleOrDefaultAsync(m => m.Id == data.BuyDocSeriesId);

                if (docSeries is null)
                {
                    transaction.Rollback();
                    ModelState.AddModelError(string.Empty, "Δεν βρέθηκε η σειρά παραστατικού");
                    return NotFound(new
                    {
                        error = "Buy Doc Series not found"
                    });
                }

                await _context.Entry(docSeries).Reference(t => t.BuyDocTypeDef).LoadAsync();
                var docTypeDef = docSeries.BuyDocTypeDef;
                //await _context.Entry(docTypeDef)
                //      .Reference(t => t.TransSupplierDef)
                //      .LoadAsync();
                await _context.Entry(docTypeDef).Reference(t => t.TransTransactorDef)
                    .LoadAsync();
                await _context.Entry(docTypeDef).Reference(t => t.TransWarehouseDef)
                    .LoadAsync();

               
                var transTransactorDef = docTypeDef.TransTransactorDef;
                //var transSupplierDef = docTypeDef.TransSupplierDef;
                var transWarehouseDef = docTypeDef.TransWarehouseDef;

                transToAttach.SectionId = section.Id;
                transToAttach.FiscalPeriodId = fiscalPeriod.Id;
                transToAttach.BuyDocTypeId = docSeries.BuyDocTypeDefId;

                _context.Entry(transToAttach).State = EntityState.Modified;
                var docId = transToAttach.Id;
                //--------------------------------------
                if (transTransactorDef.DefaultDocSeriesId > 0)
                {
                    var transTransactorDefaultSeries = await
                         _context.TransTransactorDocSeriesDefs.FirstOrDefaultAsync(p =>
                             p.Id == transTransactorDef.DefaultDocSeriesId);
                    if (transTransactorDefaultSeries == null)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Default series for transactor transaction not found");
                        return NotFound(new
                        {
                            error = "Default series for transactor transaction not found"
                        });
                    }
                    var spTransactorCreateDto = _mapper.Map<TransactorTransCreateDto>(data);
                    //Ετσι δεν μεταφέρει το Id απο το data
                    var sTransactorTransaction = _mapper.Map<TransactorTransaction>(spTransactorCreateDto);

                    sTransactorTransaction.TransactorId = data.TransactorId;
                    sTransactorTransaction.SectionId = section.Id;
                    sTransactorTransaction.TransTransactorDocTypeId = transTransactorDefaultSeries.TransTransactorDocTypeDefId;
                    sTransactorTransaction.TransTransactorDocSeriesId = transTransactorDefaultSeries.Id;
                    sTransactorTransaction.FiscalPeriodId = fiscalPeriod.Id;
                    sTransactorTransaction.CreatorId = docId;

                    switch (transTransactorDef.FinancialTransAction)
                    {
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNoChange:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNoChange;
                            sTransactorTransaction.TransDiscountAmount = 0;
                            sTransactorTransaction.TransFpaAmount = 0;
                            sTransactorTransaction.TransNetAmount = 0;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumDebit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumDebit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumCredit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumCredit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeDebit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNegativeDebit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount * -1;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa * -1;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet * -1;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeCredit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNegativeCredit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount * -1;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa * -1;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet * -1;
                            break;
                        default:
                            break;
                    }
                    _context.TransactorTransactions.Add(sTransactorTransaction);
                   
                }
                //--------------------------------------
               
                //-------------------------------------

                int warehouseSeriesId = 0;
                int warehouseTypeId = 0;

                if (transWarehouseDef.DefaultDocSeriesId > 0)
                {

                    var transWarehouseDefaultSeries =
                        await _context.TransWarehouseDocSeriesDefs.FirstOrDefaultAsync(p =>
                            p.Id == transWarehouseDef.DefaultDocSeriesId);
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
                    warehouseSeriesId = transWarehouseDef.DefaultDocSeriesId;
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
                    var buyMaterialLine = new BuyDocLine();
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
                        switch (material.MaterialNature)
                        {
                            case MaterialNatureEnum.MaterialNatureEnumUndefined:
                                throw new ArgumentOutOfRangeException();
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumMaterial:
                                warehouseTrans.InventoryAction = transWarehouseDef.MaterialInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.MaterialInventoryValueAction;

                                break;
                            case MaterialNatureEnum.MaterialNatureEnumService:
                                warehouseTrans.InventoryAction = transWarehouseDef.ServiceInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ServiceInventoryValueAction;
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumExpense:
                                warehouseTrans.InventoryAction = transWarehouseDef.ExpenseInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ExpenseInventoryValueAction;
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumIncome:
                                warehouseTrans.InventoryAction = transWarehouseDef.IncomeInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.IncomeInventoryValueAction;
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumFixedAsset:
                                warehouseTrans.InventoryAction = transWarehouseDef.FixedAssetInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.FixedAssetInventoryValueAction;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        switch (warehouseTrans.InventoryAction)
                        {
                            case InventoryActionEnum.InventoryActionEnumNoChange:
                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeIgnore;
                                warehouseTrans.TransQ1 = 0;
                                warehouseTrans.TransQ2 = 0;
                                break;
                            case InventoryActionEnum.InventoryActionEnumImport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                                warehouseTrans.Quontity1 = dataBuyDocLine.Q1;
                                warehouseTrans.Quontity2 = dataBuyDocLine.Q2;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2;
                                break;
                            case InventoryActionEnum.InventoryActionEnumExport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                                warehouseTrans.Quontity1 = dataBuyDocLine.Q1;
                                warehouseTrans.Quontity2 = dataBuyDocLine.Q2;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2;
                                break;
                            case InventoryActionEnum.InventoryActionEnumNegativeImport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                                warehouseTrans.Quontity1 = dataBuyDocLine.Q1;
                                warehouseTrans.Quontity2 = dataBuyDocLine.Q2;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1 * -1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2 * -1;
                                break;
                            case InventoryActionEnum.InventoryActionEnumNegativeExport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                                warehouseTrans.Quontity1 = dataBuyDocLine.Q1;
                                warehouseTrans.Quontity2 = dataBuyDocLine.Q2;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1 * -1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2 * -1;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }


                        switch (warehouseTrans.InventoryValueAction)
                        {
                            case InventoryValueActionEnum.InventoryValueActionEnumNoChange:
                                warehouseTrans.TransNetAmount = 0;
                                warehouseTrans.TransFpaAmount = 0;
                                warehouseTrans.TransDiscountAmount = 0;
                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumIncrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount;
                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumDecrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount;
                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumNegativeIncrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet * -1;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa * -1;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount * -1;

                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumNegativeDecrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet * -1;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa * -1;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount * -1;
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


        [HttpPost("SalesDoc")]
        public async Task<IActionResult> PostSalesDoc([FromBody] SellDocCreateAjaxDto data)
        {
            const string sectionCode = "SYS-SELL-COMBINED-SCN";

            var sessionCompanyId = HttpContext.Session.GetString("CompanyId");

            bool noSupplierTrans = false;
            bool noWarehouseTrans = false;
            var transToAttachNoLines = _mapper.Map<SellDocCreateAjaxNoLinesDto>(data);
            var transToAttach = _mapper.Map<SellDocument>(transToAttachNoLines);
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
                    _context.SellDocSeriesDefs.SingleOrDefaultAsync(m => m.Id == data.SellDocSeriesId);

                if (docSeries is null)
                {
                    transaction.Rollback();
                    ModelState.AddModelError(string.Empty, "Δεν βρέθηκε η σειρά παραστατικού");
                    return NotFound(new
                    {
                        error = "Buy Doc Series not found"
                    });
                }

                await _context.Entry(docSeries).Reference(t => t.SellDocTypeDef).LoadAsync();
                var docTypeDef = docSeries.SellDocTypeDef;
               
                await _context.Entry(docTypeDef)
                    .Reference(t => t.TransTransactorDef)
                    .LoadAsync();

                await _context.Entry(docTypeDef).Reference(t => t.TransWarehouseDef)
                    .LoadAsync();

                var transTransactorDef = docTypeDef.TransTransactorDef;
                var transWarehouseDef = docTypeDef.TransWarehouseDef;

                transToAttach.SectionId = section.Id;
                transToAttach.FiscalPeriodId = fiscalPeriod.Id;
                transToAttach.SellDocTypeId = docSeries.SellDocTypeDefId;
                _context.SellDocuments.Add(transToAttach);

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

               
                if (transTransactorDef.DefaultDocSeriesId > 0)
                {
                    var transTransactorDefaultSeries = await
                         _context.TransTransactorDocSeriesDefs.FirstOrDefaultAsync(p =>
                             p.Id == transTransactorDef.DefaultDocSeriesId);
                    if (transTransactorDefaultSeries == null)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Default series for transactor transaction not found");
                        return NotFound(new
                        {
                            error = "Default series for transactor transaction not found"
                        });
                    }
                    var sTransactorTransaction = _mapper.Map<TransactorTransaction>(data);
                    sTransactorTransaction.TransactorId = data.TransactorId;
                    sTransactorTransaction.SectionId = section.Id;
                    sTransactorTransaction.TransTransactorDocTypeId = transTransactorDefaultSeries.TransTransactorDocTypeDefId;
                    sTransactorTransaction.TransTransactorDocSeriesId = transTransactorDefaultSeries.Id;
                    sTransactorTransaction.FiscalPeriodId = fiscalPeriod.Id;
                    sTransactorTransaction.CreatorId = docId;

                    switch (transTransactorDef.FinancialTransAction)
                    {
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNoChange:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNoChange;
                            sTransactorTransaction.TransDiscountAmount = 0;
                            sTransactorTransaction.TransFpaAmount = 0;
                            sTransactorTransaction.TransNetAmount = 0;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumDebit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumDebit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumCredit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumCredit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeDebit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNegativeDebit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount * -1;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa * -1;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet * -1;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeCredit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNegativeCredit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount * -1;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa * -1;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet * -1;
                            break;
                        default:
                            break;
                    }
                    _context.TransactorTransactions.Add(sTransactorTransaction);
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
                int warehouseSeriesId = 0;
                int warehouseTypeId = 0;

                if (transWarehouseDef.DefaultDocSeriesId > 0)
                {

                    var transWarehouseDefaultSeries =
                        await _context.TransWarehouseDocSeriesDefs.FirstOrDefaultAsync(p =>
                            p.Id == transWarehouseDef.DefaultDocSeriesId);
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
                    warehouseSeriesId = transWarehouseDef.DefaultDocSeriesId;
                    warehouseTypeId = transWarehouseDefaultSeries.TransWarehouseDocTypeDefId;
                }
                else
                {
                    noWarehouseTrans = true;
                }

                foreach (var docLine in data.SellDocLines)
                {
                    var materialId = docLine.MaterialId;
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
                    var sellDocLine = new SellDocLine();
                    decimal unitPrice = docLine.Price;
                    decimal units = (decimal)docLine.Q1;
                    decimal fpaRate = (decimal)docLine.FpaRate;
                    decimal discountRate = (decimal)docLine.DiscountRate;
                    decimal lineNetAmount = unitPrice * units;
                    decimal lineDiscountAmount = lineNetAmount * discountRate;
                    decimal lineFpaAmount = (lineNetAmount - lineDiscountAmount) * fpaRate;
                    sellDocLine.UnitPrice = unitPrice;
                    sellDocLine.AmountFpa = lineFpaAmount;
                    sellDocLine.AmountNet = lineNetAmount;
                    sellDocLine.AmountDiscount = lineDiscountAmount;
                    sellDocLine.DiscountRate = discountRate;
                    sellDocLine.FpaRate = fpaRate;
                    sellDocLine.MaterialId = docLine.MaterialId;
                    sellDocLine.Quontity1 = docLine.Q1;
                    sellDocLine.Quontity2 = docLine.Q2;
                    sellDocLine.PrimaryUnitId = docLine.MainUnitId;
                    sellDocLine.SecondaryUnitId = docLine.SecUnitId;
                    sellDocLine.Factor = docLine.Factor;
                    sellDocLine.SellDocumentId = docId;
                    sellDocLine.Etiology = transToAttach.Etiology;
                    //_context.Entry(transToAttach).Entity
                    transToAttach.SellDocLines.Add(sellDocLine);
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
                        warehouseTrans.PrimaryUnitId = docLine.MainUnitId;
                        warehouseTrans.SecondaryUnitId = docLine.SecUnitId;
                        warehouseTrans.SectionId = section.Id;
                        warehouseTrans.CreatorId = transToAttach.Id;
                        warehouseTrans.TransDate = transToAttach.TransDate;
                        warehouseTrans.TransRefCode = transToAttach.TransRefCode;
                        warehouseTrans.UnitFactor = (decimal)docLine.Factor;

                        warehouseTrans.TransWarehouseDocSeriesId = warehouseSeriesId;
                        warehouseTrans.TransWarehouseDocTypeId = warehouseTypeId;

                        switch (material.MaterialNature)
                        {
                            case MaterialNatureEnum.MaterialNatureEnumUndefined:
                                throw new ArgumentOutOfRangeException();
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumMaterial:
                                warehouseTrans.InventoryAction = transWarehouseDef.MaterialInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.MaterialInventoryValueAction;

                                break;
                            case MaterialNatureEnum.MaterialNatureEnumService:
                                warehouseTrans.InventoryAction = transWarehouseDef.ServiceInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ServiceInventoryValueAction;
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumExpense:
                                warehouseTrans.InventoryAction = transWarehouseDef.ExpenseInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ExpenseInventoryValueAction;
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumIncome:
                                warehouseTrans.InventoryAction = transWarehouseDef.IncomeInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.IncomeInventoryValueAction;
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumFixedAsset:
                                warehouseTrans.InventoryAction = transWarehouseDef.FixedAssetInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.FixedAssetInventoryValueAction;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        switch (warehouseTrans.InventoryAction)
                        {
                            case InventoryActionEnum.InventoryActionEnumNoChange:
                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeIgnore;
                                warehouseTrans.TransQ1 = 0;
                                warehouseTrans.TransQ2 = 0;
                                break;
                            case InventoryActionEnum.InventoryActionEnumImport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                                warehouseTrans.Quontity1 = docLine.Q1;
                                warehouseTrans.Quontity2 = docLine.Q2;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2;
                                break;
                            case InventoryActionEnum.InventoryActionEnumExport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                                warehouseTrans.Quontity1 = docLine.Q1;
                                warehouseTrans.Quontity2 = docLine.Q2;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2;
                                break;
                            case InventoryActionEnum.InventoryActionEnumNegativeImport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                                warehouseTrans.Quontity1 = docLine.Q1;
                                warehouseTrans.Quontity2 = docLine.Q2;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1 * -1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2 * -1;
                                break;
                            case InventoryActionEnum.InventoryActionEnumNegativeExport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                                warehouseTrans.Quontity1 = docLine.Q1;
                                warehouseTrans.Quontity2 = docLine.Q2;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1 * -1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2 * -1;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }


                        switch (warehouseTrans.InventoryValueAction)
                        {
                            case InventoryValueActionEnum.InventoryValueActionEnumNoChange:
                                warehouseTrans.TransNetAmount = 0;
                                warehouseTrans.TransFpaAmount = 0;
                                warehouseTrans.TransDiscountAmount = 0;
                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumIncrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount;
                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumDecrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount;
                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumNegativeIncrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet * -1;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa * -1;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount * -1;

                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumNegativeDecrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet * -1;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa * -1;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount * -1;
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

        [HttpPut("SalesDocUpdate")]
        public async Task<IActionResult> PutSalesDoc([FromBody] SellDocModifyAjaxDto data)
        {
            const string sectionCode = "SYS-SELL-COMBINED-SCN";
            bool noWarehouseTrans;

            var transToAttachNoLines = _mapper.Map<SellDocModifyAjaxNoLinesDto>(data);
            var transToAttach = _mapper.Map<SellDocument>(transToAttachNoLines);
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
                _context.SellDocLines.RemoveRange(_context.SellDocLines.Where(p => p.SellDocumentId == data.Id));
                _context.TransactorTransactions.RemoveRange(_context.TransactorTransactions.Where(p => p.SectionId == section.Id && p.CreatorId == data.Id));
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
                    _context.SellDocSeriesDefs.SingleOrDefaultAsync(m => m.Id == data.SellDocSeriesId);

                if (docSeries is null)
                {
                    transaction.Rollback();
                    ModelState.AddModelError(string.Empty, "Δεν βρέθηκε η σειρά παραστατικού");
                    return NotFound(new
                    {
                        error = "Buy Doc Series not found"
                    });
                }

                await _context.Entry(docSeries).Reference(t => t.SellDocTypeDef).LoadAsync();
                var docTypeDef = docSeries.SellDocTypeDef;
                await _context.Entry(docTypeDef)
                      .Reference(t => t.TransTransactorDef)
                      .LoadAsync();
                await _context.Entry(docTypeDef).Reference(t => t.TransTransactorDef)
                    .LoadAsync();
                await _context.Entry(docTypeDef).Reference(t => t.TransWarehouseDef)
                    .LoadAsync();


                var transTransactorDef = docTypeDef.TransTransactorDef;
               // var transSupplierDef = docTypeDef.TransSupplierDef;
                var transWarehouseDef = docTypeDef.TransWarehouseDef;

                transToAttach.SectionId = section.Id;
                transToAttach.FiscalPeriodId = fiscalPeriod.Id;
                transToAttach.SellDocTypeId = docSeries.SellDocTypeDefId;

                _context.Entry(transToAttach).State = EntityState.Modified;
                var docId = transToAttach.Id;
                //--------------------------------------
                if (transTransactorDef.DefaultDocSeriesId > 0)
                {
                    var transTransactorDefaultSeries = await
                         _context.TransTransactorDocSeriesDefs.FirstOrDefaultAsync(p =>
                             p.Id == transTransactorDef.DefaultDocSeriesId);
                    if (transTransactorDefaultSeries == null)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Default series for transactor transaction not found");
                        return NotFound(new
                        {
                            error = "Default series for transactor transaction not found"
                        });
                    }
                    var spTransactorCreateDto = _mapper.Map<TransactorTransCreateDto>(data);
                    //Ετσι δεν μεταφέρει το Id απο το data
                    var sTransactorTransaction = _mapper.Map<TransactorTransaction>(spTransactorCreateDto);

                    sTransactorTransaction.TransactorId = data.TransactorId;
                    sTransactorTransaction.SectionId = section.Id;
                    sTransactorTransaction.TransTransactorDocTypeId = transTransactorDefaultSeries.TransTransactorDocTypeDefId;
                    sTransactorTransaction.TransTransactorDocSeriesId = transTransactorDefaultSeries.Id;
                    sTransactorTransaction.FiscalPeriodId = fiscalPeriod.Id;
                    sTransactorTransaction.CreatorId = docId;

                    switch (transTransactorDef.FinancialTransAction)
                    {
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNoChange:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNoChange;
                            sTransactorTransaction.TransDiscountAmount = 0;
                            sTransactorTransaction.TransFpaAmount = 0;
                            sTransactorTransaction.TransNetAmount = 0;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumDebit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumDebit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumCredit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumCredit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeDebit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNegativeDebit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount * -1;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa * -1;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet * -1;
                            break;
                        case InfoSystem.Domain.FinConfig.FinActionsEnum.FinActionsEnumNegativeCredit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNegativeCredit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount * -1;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa * -1;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet * -1;
                            break;
                        default:
                            break;
                    }
                    _context.TransactorTransactions.Add(sTransactorTransaction);

                }
               

                int warehouseSeriesId = 0;
                int warehouseTypeId = 0;

                if (transWarehouseDef.DefaultDocSeriesId > 0)
                {

                    var transWarehouseDefaultSeries =
                        await _context.TransWarehouseDocSeriesDefs.FirstOrDefaultAsync(p =>
                            p.Id == transWarehouseDef.DefaultDocSeriesId);
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
                    warehouseSeriesId = transWarehouseDef.DefaultDocSeriesId;
                    warehouseTypeId = transWarehouseDefaultSeries.TransWarehouseDocTypeDefId;
                }
                else
                {
                    noWarehouseTrans = true;
                }
                foreach (var docLine in data.SellDocLines)
                {
                    var materialId = docLine.MaterialId;
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
                    var sellDocLine = new SellDocLine();
                    decimal unitPrice = docLine.Price;
                    decimal units = (decimal)docLine.Q1;
                    decimal fpaRate = (decimal)docLine.FpaRate;
                    decimal discountRate = (decimal)docLine.DiscountRate;
                    decimal lineNetAmount = unitPrice * units;
                    decimal lineDiscountAmount = lineNetAmount * discountRate;
                    decimal lineFpaAmount = (lineNetAmount - lineDiscountAmount) * fpaRate;
                    sellDocLine.UnitPrice = unitPrice;
                    sellDocLine.AmountFpa = lineFpaAmount;
                    sellDocLine.AmountNet = lineNetAmount;
                    sellDocLine.AmountDiscount = lineDiscountAmount;
                    sellDocLine.DiscountRate = discountRate;
                    sellDocLine.FpaRate = fpaRate;
                    sellDocLine.MaterialId = docLine.MaterialId;
                    sellDocLine.Quontity1 = docLine.Q1;
                    sellDocLine.Quontity2 = docLine.Q2;
                    sellDocLine.PrimaryUnitId = docLine.MainUnitId;
                    sellDocLine.SecondaryUnitId = docLine.SecUnitId;
                    sellDocLine.Factor = docLine.Factor;
                    sellDocLine.SellDocumentId = docId;
                    sellDocLine.Etiology = transToAttach.Etiology;
                    //_context.Entry(transToAttach).Entity
                    transToAttach.SellDocLines.Add(sellDocLine);
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
                        warehouseTrans.PrimaryUnitId = docLine.MainUnitId;
                        warehouseTrans.SecondaryUnitId = docLine.SecUnitId;
                        warehouseTrans.SectionId = section.Id;
                        warehouseTrans.CreatorId = transToAttach.Id;
                        warehouseTrans.TransDate = transToAttach.TransDate;
                        warehouseTrans.TransRefCode = transToAttach.TransRefCode;
                        warehouseTrans.UnitFactor = (decimal)docLine.Factor;

                        warehouseTrans.TransWarehouseDocSeriesId = warehouseSeriesId;
                        warehouseTrans.TransWarehouseDocTypeId = warehouseTypeId;
                        switch (material.MaterialNature)
                        {
                            case MaterialNatureEnum.MaterialNatureEnumUndefined:
                                throw new ArgumentOutOfRangeException();
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumMaterial:
                                warehouseTrans.InventoryAction = transWarehouseDef.MaterialInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.MaterialInventoryValueAction;

                                break;
                            case MaterialNatureEnum.MaterialNatureEnumService:
                                warehouseTrans.InventoryAction = transWarehouseDef.ServiceInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ServiceInventoryValueAction;
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumExpense:
                                warehouseTrans.InventoryAction = transWarehouseDef.ExpenseInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ExpenseInventoryValueAction;
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumIncome:
                                warehouseTrans.InventoryAction = transWarehouseDef.IncomeInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.IncomeInventoryValueAction;
                                break;
                            case MaterialNatureEnum.MaterialNatureEnumFixedAsset:
                                warehouseTrans.InventoryAction = transWarehouseDef.FixedAssetInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.FixedAssetInventoryValueAction;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        switch (warehouseTrans.InventoryAction)
                        {
                            case InventoryActionEnum.InventoryActionEnumNoChange:
                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeIgnore;
                                warehouseTrans.TransQ1 = 0;
                                warehouseTrans.TransQ2 = 0;
                                break;
                            case InventoryActionEnum.InventoryActionEnumImport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                                warehouseTrans.Quontity1 = docLine.Q1;
                                warehouseTrans.Quontity2 = docLine.Q2;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2;
                                break;
                            case InventoryActionEnum.InventoryActionEnumExport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                                warehouseTrans.Quontity1 = docLine.Q1;
                                warehouseTrans.Quontity2 = docLine.Q2;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2;
                                break;
                            case InventoryActionEnum.InventoryActionEnumNegativeImport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeImport;
                                warehouseTrans.Quontity1 = docLine.Q1;
                                warehouseTrans.Quontity2 = docLine.Q2;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1 * -1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2 * -1;
                                break;
                            case InventoryActionEnum.InventoryActionEnumNegativeExport:

                                warehouseTrans.TransactionType =
                                    WarehouseTransactionTypeEnum.WarehouseTransactionTypeExport;
                                warehouseTrans.Quontity1 = docLine.Q1;
                                warehouseTrans.Quontity2 = docLine.Q2;
                                warehouseTrans.TransQ1 = (decimal)warehouseTrans.Quontity1 * -1;
                                warehouseTrans.TransQ2 = (decimal)warehouseTrans.Quontity2 * -1;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }


                        switch (warehouseTrans.InventoryValueAction)
                        {
                            case InventoryValueActionEnum.InventoryValueActionEnumNoChange:
                                warehouseTrans.TransNetAmount = 0;
                                warehouseTrans.TransFpaAmount = 0;
                                warehouseTrans.TransDiscountAmount = 0;
                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumIncrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount;
                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumDecrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount;
                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumNegativeIncrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet * -1;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa * -1;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount * -1;

                                break;
                            case InventoryValueActionEnum.InventoryValueActionEnumNegativeDecrease:
                                warehouseTrans.TransNetAmount = warehouseTrans.AmountNet * -1;
                                warehouseTrans.TransFpaAmount = warehouseTrans.AmountFpa * -1;
                                warehouseTrans.TransDiscountAmount = warehouseTrans.AmountDiscount * -1;
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