using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GrKouk.InfoSystem.Definitions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.SellDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.TransactorTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseItems;
using GrKouk.WebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace GrKouk.WebRazor.Controllers
{
    [Authorize(Roles = "Admin")]
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

        // GET: api/WarehouseItems
        [HttpGet]
        public IEnumerable<WarehouseItem> GetMaterials()
        {
            return _context.WarehouseItems;
        }
        [HttpGet("SetCompanyInSession")]
        public IActionResult CompanyInSession(string companyId)
        {
            HttpContext.Session.SetString ("CompanyId",companyId);
            return Ok(new {});
        }
        [HttpGet("SetBuySeriesInSession")]
        public IActionResult BuySeriesInSession(string seriesId)
        {
            HttpContext.Session.SetString("BuySeriesId", seriesId);
            return Ok(new { });
        }
        [HttpGet("SetSaleSeriesInSession")]
        public IActionResult SaleSeriesInSession(string seriesId)
        {
            HttpContext.Session.SetString("SalesSeriesId", seriesId);
            return Ok(new { });
        }
        [HttpGet("SeekBarcode")]
        public async Task<IActionResult> GetMaterialFromBarcode(string barcode)
        {
            //var sessionCompanyId = HttpContext.Session.GetString("CompanyId");
            var materials = await _context.WarehouseItemsCodes
                .Include(p => p.WarehouseItem).ThenInclude(p=>p.FpaDef)
                .Include(p => p.WarehouseItem).ThenInclude(p => p.MainMeasureUnit)
                .Include(p => p.WarehouseItem).ThenInclude(p => p.SecondaryMeasureUnit)
                .Include(p => p.WarehouseItem).ThenInclude(p => p.BuyMeasureUnit)
                .Where(p => p.Code == barcode && p.CodeType == WarehouseItemCodeTypeEnum.CodeTypeEnumBarcode)
                .ToListAsync();

                //.ProjectTo<WarehouseItemSearchListDto>(_mapper.ConfigurationProvider)
                //.Select(p => new { label = p.Label, value = p.Id }).ToListAsync();

            if (materials == null)
            {
                return NotFound( new
                {
                    Error="WarehouseItem Not Found"
                });
            }

            if (materials.Count>1)
            {
                return NotFound(new
                {
                    Error = "More than one material found"
                });
            }

            var material = materials[0].WarehouseItem;
            
            var usedUnit = materials[0].CodeUsedUnit;
            double unitFactor;
            string unitToUse;
            switch (usedUnit)
            {
                case WarehouseItemCodeUsedUnitEnum.CodeUsedUnitEnumMain:
                    unitFactor = 1;
                    unitToUse = "MAIN";
                    break;
                case WarehouseItemCodeUsedUnitEnum.CodeUsedUnitEnumSecondary:
                    unitFactor = material.SecondaryUnitToMainRate;
                    unitToUse = "SEC";
                    break;
                case WarehouseItemCodeUsedUnitEnum.CodeUsedUnitEnumBuy:
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
                Factor=unitFactor,
                mainUnitId = material.MainMeasureUnitId,
                secUnitId = material.SecondaryMeasureUnitId,
                buyUnitId = material.BuyMeasureUnitId,
                mainUnitCode = material.MainMeasureUnit.Code,
                secUnitCode = material.SecondaryMeasureUnit.Code,
                buyUnitCode = material.BuyMeasureUnit.Code,
                factorSeq = material.SecondaryUnitToMainRate,
                factorBuy = material.BuyUnitToMainRate,

            });
        }
        [HttpGet("SearchWarehouseItemsForBuy")]
        public async Task<IActionResult> GetWarehouseItemsForBuy(string term)
        {
            var sessionCompanyId = HttpContext.Session.GetString("CompanyId");
            var sessionSeriesId= HttpContext.Session.GetString("BuySeriesId");
            IQueryable<WarehouseItem> fullListIq = _context.WarehouseItems;
            if (sessionCompanyId != null)
            {
                bool isInt = int.TryParse(sessionCompanyId, out var companyId);
                if (isInt)
                {
                    if (companyId>1)
                    {
                        //Not all companies 
                       fullListIq= fullListIq.Where(p => p.CompanyId == companyId || p.CompanyId==1);
                    }
                }
            }

            if (sessionSeriesId != null)
            {
                int seriesId;
                bool isInt = int.TryParse(sessionSeriesId, out seriesId);
                var series = await _context.BuyDocSeriesDefs
                    .Include(p => p.BuyDocTypeDef)
                    .SingleOrDefaultAsync(p => p.Id == seriesId);
                if (series!=null)
                {
                    var docType = series.BuyDocTypeDef;
                    var itemNatures = docType.SelectedWarehouseItemNatures;
                    if (!string.IsNullOrEmpty(itemNatures))
                    {
                        var natures = Array.ConvertAll(docType.SelectedWarehouseItemNatures.Split(","), int.Parse);
                        //var natures = docType.SelectedWarehouseItemNatures;
                        fullListIq = fullListIq.Where(p => natures.Contains( (int)p.WarehouseItemNature));
                    }
                }
            }

            fullListIq = fullListIq.Where(p => p.Active);
            fullListIq = fullListIq.Where(p => p.Name.Contains(term));
            var materials = await fullListIq
                .ProjectTo<WarehouseItemSearchListDto>(_mapper.ConfigurationProvider)
                .Select(p => new { label = p.Label, value = p.Id }).ToListAsync();

            if (materials == null)
            {
                return NotFound();
            }

            return Ok(materials);
        }
        [HttpGet("SearchWarehouseItemsForSale")]
        public async Task<IActionResult> GetWarehouseItemsForSale(string term)
        {
            var sessionCompanyId = HttpContext.Session.GetString("CompanyId");
            var sessionSeriesId = HttpContext.Session.GetString("BuySeriesId");
            IQueryable<WarehouseItem> fullListIq = _context.WarehouseItems;
            if (sessionCompanyId != null)
            {
                int companyId;
                bool isInt = int.TryParse(sessionCompanyId, out companyId);
                if (isInt)
                {
                    if (companyId > 1)
                    {
                        //Not all companies 
                       fullListIq= fullListIq.Where(p => p.CompanyId == companyId || p.CompanyId == 1);
                    }
                }
            }

            if (sessionSeriesId != null)
            {
                int seriesId;
                bool isInt = int.TryParse(sessionSeriesId, out seriesId);
                var series = await _context.SellDocSeriesDefs
                    .Include(p => p.SellDocTypeDef)
                    .SingleOrDefaultAsync(p => p.Id == seriesId);
                if (series != null)
                {
                    var docType = series.SellDocTypeDef;
                    var itemNatures = docType.SelectedWarehouseItemNatures;
                    if (!string.IsNullOrEmpty(itemNatures))
                    {
                        var natures = Array.ConvertAll(docType.SelectedWarehouseItemNatures.Split(","), int.Parse);
                        //var natures = docType.SelectedWarehouseItemNatures;
                        fullListIq = fullListIq.Where(p => natures.Contains((int)p.WarehouseItemNature));
                    }
                }
            }

            fullListIq = fullListIq.Where(p => p.Name.Contains(term));
            //var materials = await _context.WarehouseItems.Where(p => p.Name.Contains(term) )
            //    .ProjectTo<WarehouseItemSearchListDto>(_mapper.ConfigurationProvider)
            //    .Select(p => new { label = p.Label, value = p.Id }).ToListAsync();
            var materials = await fullListIq
                .ProjectTo<WarehouseItemSearchListDto>(_mapper.ConfigurationProvider)
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

            var materials = await _context.WarehouseItems.Where(p => p.Name.Contains(term) && p.WarehouseItemNature == WarehouseItemNatureEnum.WarehouseItemNatureService)
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

            var materials = await _context.WarehouseItems.Where(p => p.Name.Contains(term) && p.WarehouseItemNature == WarehouseItemNatureEnum.WarehouseItemNatureExpense)
                .Select(p => new { label = p.Name, value = p.Id }).ToListAsync();

            if (materials == null)
            {
                return NotFound();
            }

            return Ok(materials);
        }
        [HttpGet("materialdata")]
        public async Task<IActionResult> GetMaterialData(int warehouseItemId)
        {
            //TODO: Να βρίσκει τιμές μόνο για κινήσεις αγοράς ισως LasrPriceImport LastPriceExport???
            var lastPr = await _context.WarehouseTransactions.OrderByDescending(p=>p.TransDate)
                .Where(m => m.WarehouseItemId == warehouseItemId)
                .Select(k => new
                {
                    LastPrice = k.UnitPrice
                }).FirstOrDefaultAsync();

            var lastPrice = lastPr?.LastPrice ?? 0;

            var materialData = await _context.WarehouseItems.Where(p => p.Id == warehouseItemId && p.Active)
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
                    error = "WarehouseItem not found "
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

            //BuySeriesInSession(seriesId.ToString());
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
           // bool noSupplierTrans = false;
            bool noWarehouseTrans = false;


            BuyDocCreateAjaxNoLinesDto transToAttachNoLines;
            BuyDocument transToAttach;
            DateTime dateOfTrans;

            if (data == null)
            {
                return BadRequest(new
                {
                    error = "Empty request data"
                });
            }
            try
            {
                 transToAttachNoLines = _mapper.Map<BuyDocCreateAjaxNoLinesDto>(data);
                 transToAttach = _mapper.Map<BuyDocument>(transToAttachNoLines);
                 dateOfTrans = data.TransDate;
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    error = e.Message
                });
            }
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
                    string msg = e.InnerException.Message;
                    return BadRequest(new
                    {
                        error = e.Message + " " + msg
                    });

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
                        case FinActionsEnum.FinActionsEnumNoChange:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNoChange;
                            sTransactorTransaction.TransDiscountAmount = 0;
                            sTransactorTransaction.TransFpaAmount = 0;
                            sTransactorTransaction.TransNetAmount = 0;
                            break;
                        case FinActionsEnum.FinActionsEnumDebit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumDebit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                            break;
                        case FinActionsEnum.FinActionsEnumCredit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumCredit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                            break;
                        case FinActionsEnum.FinActionsEnumNegativeDebit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNegativeDebit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount*-1;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa*-1;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet*-1;
                            break;
                        case FinActionsEnum.FinActionsEnumNegativeCredit:
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
                        string msg = e.InnerException.Message;
                        return BadRequest(new
                        {
                            error = e.Message + " " + msg
                        });
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
                    var warehouseItemId = dataBuyDocLine.WarehouseItemId;
                    var material = await _context.WarehouseItems.SingleOrDefaultAsync(p => p.Id == warehouseItemId);
                    if (material is null)
                    {
                        //Handle error
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Doc Line error null WarehouseItem");
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
                    buyMaterialLine.WarehouseItemId = dataBuyDocLine.WarehouseItemId;
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

                        warehouseTrans.WarehouseItemId = warehouseItemId;
                        warehouseTrans.PrimaryUnitId = dataBuyDocLine.MainUnitId;
                        warehouseTrans.SecondaryUnitId = dataBuyDocLine.SecUnitId;
                        warehouseTrans.SectionId = section.Id;
                        warehouseTrans.CreatorId = transToAttach.Id;
                        warehouseTrans.TransDate = transToAttach.TransDate;
                        warehouseTrans.TransRefCode = transToAttach.TransRefCode;
                        warehouseTrans.UnitFactor = (decimal)dataBuyDocLine.Factor;

                        warehouseTrans.TransWarehouseDocSeriesId = warehouseSeriesId;
                        warehouseTrans.TransWarehouseDocTypeId = warehouseTypeId;

                        switch (material.WarehouseItemNature)
                        {
                            case WarehouseItemNatureEnum.WarehouseItemNatureUndefined:
                                throw new ArgumentOutOfRangeException();
                            case WarehouseItemNatureEnum.WarehouseItemNatureMaterial:
                                warehouseTrans.InventoryAction = transWarehouseDef.MaterialInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.MaterialInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureService:
                                warehouseTrans.InventoryAction = transWarehouseDef.ServiceInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ServiceInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureExpense:
                                warehouseTrans.InventoryAction = transWarehouseDef.ExpenseInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ExpenseInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureIncome:
                                warehouseTrans.InventoryAction = transWarehouseDef.IncomeInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.IncomeInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureFixedAsset:
                                warehouseTrans.InventoryAction = transWarehouseDef.FixedAssetInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.FixedAssetInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureRawMaterial:
                                warehouseTrans.InventoryAction = transWarehouseDef.RawMaterialInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.RawMaterialInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
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
                    transaction.Rollback();
                    string msg = e.InnerException.Message;
                    return BadRequest(new
                    {
                        error = e.Message + " " + msg
                    });
                }
            }

            return Ok(new { });
        }

        [HttpPut("MaterialBuyDocUpdate")]
        public async Task<IActionResult> PutMaterialBuyDoc([FromBody] BuyDocModifyAjaxDto data)
        {
            const string sectionCode = "SYS-BUY-MATERIALS-SCN";
            bool noWarehouseTrans;

            BuyDocModifyAjaxNoLinesDto transToAttachNoLines;
            BuyDocument transToAttach;
            DateTime dateOfTrans;
            if (data == null)
            {
                return BadRequest(new
                {
                    error = "Empty request data"
                });
            }
            try
            {
                transToAttachNoLines = _mapper.Map<BuyDocModifyAjaxNoLinesDto>(data);
                transToAttach = _mapper.Map<BuyDocument>(transToAttachNoLines);
                dateOfTrans = data.TransDate;
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    error = e.Message
                });
            }
           
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

                    try
                    {
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
                            case FinActionsEnum.FinActionsEnumNoChange:
                                sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNoChange;
                                sTransactorTransaction.TransDiscountAmount = 0;
                                sTransactorTransaction.TransFpaAmount = 0;
                                sTransactorTransaction.TransNetAmount = 0;
                                break;
                            case FinActionsEnum.FinActionsEnumDebit:
                                sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumDebit;
                                sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                                sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                                sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                                break;
                            case FinActionsEnum.FinActionsEnumCredit:
                                sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumCredit;
                                sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                                sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                                sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                                break;
                            case FinActionsEnum.FinActionsEnumNegativeDebit:
                                sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNegativeDebit;
                                sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount * -1;
                                sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa * -1;
                                sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet * -1;
                                break;
                            case FinActionsEnum.FinActionsEnumNegativeCredit:
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
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        string msg = e.InnerException.Message;
                        return BadRequest(new
                        {
                            error = e.Message + " " + msg
                        });

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
                foreach (var dataBuyDocLine in data.BuyDocLines)
                {
                    var warehouseItemId = dataBuyDocLine.WarehouseItemId;
                    var material = await _context.WarehouseItems.SingleOrDefaultAsync(p => p.Id == warehouseItemId);
                    if (material is null)
                    {
                        //Handle error
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Doc Line error null WarehouseItem");
                        return NotFound(new
                        {
                            error = "Could not locate material in Doc Line "
                        });
                    }
                    #region MaterialLine
                    var warehouseItemLine = new BuyDocLine();
                    decimal unitPrice = dataBuyDocLine.Price;
                    decimal units = (decimal)dataBuyDocLine.Q1;
                    decimal fpaRate = (decimal)dataBuyDocLine.FpaRate;
                    decimal discountRate = (decimal)dataBuyDocLine.DiscountRate;
                    decimal lineNetAmount = unitPrice * units;
                    decimal lineDiscountAmount = lineNetAmount * discountRate;
                    decimal lineFpaAmount = (lineNetAmount - lineDiscountAmount) * fpaRate;
                    warehouseItemLine.UnitPrice = unitPrice;
                    warehouseItemLine.AmountFpa = lineFpaAmount;
                    warehouseItemLine.AmountNet = lineNetAmount;
                    warehouseItemLine.AmountDiscount = lineDiscountAmount;
                    warehouseItemLine.DiscountRate = discountRate;
                    warehouseItemLine.FpaRate = fpaRate;
                    warehouseItemLine.WarehouseItemId = dataBuyDocLine.WarehouseItemId;
                    warehouseItemLine.Quontity1 = dataBuyDocLine.Q1;
                    warehouseItemLine.Quontity2 = dataBuyDocLine.Q2;
                    warehouseItemLine.PrimaryUnitId = dataBuyDocLine.MainUnitId;
                    warehouseItemLine.SecondaryUnitId = dataBuyDocLine.SecUnitId;
                    warehouseItemLine.Factor = dataBuyDocLine.Factor;
                    warehouseItemLine.BuyDocumentId = docId;
                    warehouseItemLine.Etiology = transToAttach.Etiology;
                    //_context.Entry(transToAttach).Entity

                    try
                    {
                        transToAttach.BuyDocLines.Add(warehouseItemLine);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        string msg = e.InnerException.Message;
                        return BadRequest(new
                        {
                            error = e.Message + " " + msg
                        });
                    }
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

                        warehouseTrans.WarehouseItemId = warehouseItemId;
                        warehouseTrans.PrimaryUnitId = dataBuyDocLine.MainUnitId;
                        warehouseTrans.SecondaryUnitId = dataBuyDocLine.SecUnitId;
                        warehouseTrans.SectionId = section.Id;
                        warehouseTrans.CreatorId = transToAttach.Id;
                        warehouseTrans.TransDate = transToAttach.TransDate;
                        warehouseTrans.TransRefCode = transToAttach.TransRefCode;
                        warehouseTrans.UnitFactor = (decimal)dataBuyDocLine.Factor;

                        warehouseTrans.TransWarehouseDocSeriesId = warehouseSeriesId;
                        warehouseTrans.TransWarehouseDocTypeId = warehouseTypeId;
                        switch (material.WarehouseItemNature)
                        {
                            case WarehouseItemNatureEnum.WarehouseItemNatureUndefined:
                                throw new ArgumentOutOfRangeException();
                            case WarehouseItemNatureEnum.WarehouseItemNatureMaterial:
                                warehouseTrans.InventoryAction = transWarehouseDef.MaterialInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.MaterialInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureService:
                                warehouseTrans.InventoryAction = transWarehouseDef.ServiceInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ServiceInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureExpense:
                                warehouseTrans.InventoryAction = transWarehouseDef.ExpenseInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ExpenseInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureIncome:
                                warehouseTrans.InventoryAction = transWarehouseDef.IncomeInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.IncomeInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureFixedAsset:
                                warehouseTrans.InventoryAction = transWarehouseDef.FixedAssetInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.FixedAssetInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureRawMaterial:
                                warehouseTrans.InventoryAction = transWarehouseDef.RawMaterialInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.RawMaterialInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
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

                      
                        try
                        {
                            _context.WarehouseTransactions.Add(warehouseTrans);
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            string msg = e.InnerException.Message;
                            return BadRequest(new
                            {
                                error = e.Message + " " + msg
                            });
                        }
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

                    transaction.Rollback();
                    string msg = e.InnerException.Message;
                    return BadRequest(new
                    {
                        error = e.Message + " " + msg
                    });
                }
            }

            return Ok(new { });


        }


        [HttpPost("SalesDoc")]
        public async Task<IActionResult> PostSalesDoc([FromBody] SellDocCreateAjaxDto data)
        {
            const string sectionCode = "SYS-SELL-COMBINED-SCN";

            var sessionCompanyId = HttpContext.Session.GetString("CompanyId");

           // bool noSupplierTrans = false;
            bool noWarehouseTrans = false;

            SellDocCreateAjaxNoLinesDto transToAttachNoLines;
            SellDocument transToAttach;
            DateTime dateOfTrans;

            if (data == null)
            {
                return BadRequest(new
                {
                    error = "Empty request data"
                });
            }
            try
            {
                transToAttachNoLines = _mapper.Map<SellDocCreateAjaxNoLinesDto>(data);
                transToAttach = _mapper.Map<SellDocument>(transToAttachNoLines);
                dateOfTrans = data.TransDate;
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    error = e.Message
                });
            }
            
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
                    string msg = e.InnerException.Message;
                    return BadRequest(new
                    {
                        error = e.Message + " " + msg
                    });
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
                        case FinActionsEnum.FinActionsEnumNoChange:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNoChange;
                            sTransactorTransaction.TransDiscountAmount = 0;
                            sTransactorTransaction.TransFpaAmount = 0;
                            sTransactorTransaction.TransNetAmount = 0;
                            break;
                        case FinActionsEnum.FinActionsEnumDebit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumDebit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                            break;
                        case FinActionsEnum.FinActionsEnumCredit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumCredit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                            break;
                        case FinActionsEnum.FinActionsEnumNegativeDebit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNegativeDebit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount * -1;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa * -1;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet * -1;
                            break;
                        case FinActionsEnum.FinActionsEnumNegativeCredit:
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
                        transaction.Rollback();
                        string msg = e.InnerException.Message;
                        return BadRequest(new
                        {
                            error = e.Message + " " + msg
                        });
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
                    var warehouseItemId = docLine.WarehouseItemId;
                    var material = await _context.WarehouseItems.SingleOrDefaultAsync(p => p.Id == warehouseItemId);
                    if (material is null)
                    {
                        //Handle error
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Doc Line error null WarehouseItem");
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
                    sellDocLine.WarehouseItemId = docLine.WarehouseItemId;
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

                        warehouseTrans.WarehouseItemId = warehouseItemId;
                        warehouseTrans.PrimaryUnitId = docLine.MainUnitId;
                        warehouseTrans.SecondaryUnitId = docLine.SecUnitId;
                        warehouseTrans.SectionId = section.Id;
                        warehouseTrans.CreatorId = transToAttach.Id;
                        warehouseTrans.TransDate = transToAttach.TransDate;
                        warehouseTrans.TransRefCode = transToAttach.TransRefCode;
                        warehouseTrans.UnitFactor = (decimal)docLine.Factor;

                        warehouseTrans.TransWarehouseDocSeriesId = warehouseSeriesId;
                        warehouseTrans.TransWarehouseDocTypeId = warehouseTypeId;

                        switch (material.WarehouseItemNature)
                        {
                            case WarehouseItemNatureEnum.WarehouseItemNatureUndefined:
                                throw new ArgumentOutOfRangeException();
                            case WarehouseItemNatureEnum.WarehouseItemNatureMaterial:
                                warehouseTrans.InventoryAction = transWarehouseDef.MaterialInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.MaterialInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;

                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureService:
                                warehouseTrans.InventoryAction = transWarehouseDef.ServiceInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ServiceInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureExpense:
                                warehouseTrans.InventoryAction = transWarehouseDef.ExpenseInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ExpenseInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureIncome:
                                warehouseTrans.InventoryAction = transWarehouseDef.IncomeInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.IncomeInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureFixedAsset:
                                warehouseTrans.InventoryAction = transWarehouseDef.FixedAssetInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.FixedAssetInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureRawMaterial:
                                warehouseTrans.InventoryAction = transWarehouseDef.RawMaterialInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.RawMaterialInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
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
                    transaction.Rollback();
                    string msg = e.InnerException.Message;
                    return BadRequest(new
                    {
                        error = e.Message + " " + msg
                    });
                }
            }

            return Ok(new { });
        }

        [HttpPut("SalesDocUpdate")]
        public async Task<IActionResult> PutSalesDoc([FromBody] SellDocModifyAjaxDto data)
        {
            const string sectionCode = "SYS-SELL-COMBINED-SCN";
            bool noWarehouseTrans;

            SellDocModifyAjaxNoLinesDto transToAttachNoLines;
            SellDocument transToAttach;
            DateTime dateOfTrans;
            if (data == null)
            {
                return BadRequest(new
                {
                    error = "Empty request data"
                });
            }
            try
            {
                transToAttachNoLines = _mapper.Map<SellDocModifyAjaxNoLinesDto>(data);
                transToAttach = _mapper.Map<SellDocument>(transToAttachNoLines);
                dateOfTrans = data.TransDate;
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    error = e.Message
                });
            }
            
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
                        case InfoSystem.Definitions.FinActionsEnum.FinActionsEnumNoChange:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNoChange;
                            sTransactorTransaction.TransDiscountAmount = 0;
                            sTransactorTransaction.TransFpaAmount = 0;
                            sTransactorTransaction.TransNetAmount = 0;
                            break;
                        case InfoSystem.Definitions.FinActionsEnum.FinActionsEnumDebit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumDebit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                            break;
                        case InfoSystem.Definitions.FinActionsEnum.FinActionsEnumCredit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumCredit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet;
                            break;
                        case InfoSystem.Definitions.FinActionsEnum.FinActionsEnumNegativeDebit:
                            sTransactorTransaction.FinancialAction = FinActionsEnum.FinActionsEnumNegativeDebit;
                            sTransactorTransaction.TransDiscountAmount = sTransactorTransaction.AmountDiscount * -1;
                            sTransactorTransaction.TransFpaAmount = sTransactorTransaction.AmountFpa * -1;
                            sTransactorTransaction.TransNetAmount = sTransactorTransaction.AmountNet * -1;
                            break;
                        case InfoSystem.Definitions.FinActionsEnum.FinActionsEnumNegativeCredit:
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
                    var warehouseItemId = docLine.WarehouseItemId;
                    var material = await _context.WarehouseItems.SingleOrDefaultAsync(p => p.Id == warehouseItemId);
                    if (material is null)
                    {
                        //Handle error
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Doc Line error null WarehouseItem");
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
                    sellDocLine.WarehouseItemId = docLine.WarehouseItemId;
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

                        warehouseTrans.WarehouseItemId = warehouseItemId;
                        warehouseTrans.PrimaryUnitId = docLine.MainUnitId;
                        warehouseTrans.SecondaryUnitId = docLine.SecUnitId;
                        warehouseTrans.SectionId = section.Id;
                        warehouseTrans.CreatorId = transToAttach.Id;
                        warehouseTrans.TransDate = transToAttach.TransDate;
                        warehouseTrans.TransRefCode = transToAttach.TransRefCode;
                        warehouseTrans.UnitFactor = (decimal)docLine.Factor;

                        warehouseTrans.TransWarehouseDocSeriesId = warehouseSeriesId;
                        warehouseTrans.TransWarehouseDocTypeId = warehouseTypeId;
                        switch (material.WarehouseItemNature)
                        {
                            case WarehouseItemNatureEnum.WarehouseItemNatureUndefined:
                                throw new ArgumentOutOfRangeException();
                            case WarehouseItemNatureEnum.WarehouseItemNatureMaterial:
                                warehouseTrans.InventoryAction = transWarehouseDef.MaterialInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.MaterialInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureService:
                                warehouseTrans.InventoryAction = transWarehouseDef.ServiceInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ServiceInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureExpense:
                                warehouseTrans.InventoryAction = transWarehouseDef.ExpenseInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.ExpenseInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureIncome:
                                warehouseTrans.InventoryAction = transWarehouseDef.IncomeInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.IncomeInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureFixedAsset:
                                warehouseTrans.InventoryAction = transWarehouseDef.FixedAssetInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.FixedAssetInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
                                break;
                            case WarehouseItemNatureEnum.WarehouseItemNatureRawMaterial:
                                warehouseTrans.InventoryAction = transWarehouseDef.RawMaterialInventoryAction;
                                warehouseTrans.InventoryValueAction = transWarehouseDef.RawMaterialInventoryValueAction;
                                warehouseTrans.InvoicedVolumeAction = transWarehouseDef.MaterialInvoicedVolumeAction;
                                warehouseTrans.InvoicedValueAction = transWarehouseDef.MaterialInvoicedValueAction;
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
                    transaction.Rollback();
                    string msg = e.InnerException.Message;
                    return BadRequest(new
                    {
                        error = e.Message + " " + msg
                    });
                }
            }

            return Ok(new { });


        }
    }
}