﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.MediaEntities;
using GrKouk.InfoSystem.Domain.RecurringTransactions;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using GrKouk.InfoSystem.Dtos.WebDtos;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.CashRegister;
using GrKouk.InfoSystem.Dtos.WebDtos.CustomerTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.Diaries;
using GrKouk.InfoSystem.Dtos.WebDtos.Media;
using GrKouk.InfoSystem.Dtos.WebDtos.ProductRecipies;
using GrKouk.InfoSystem.Dtos.WebDtos.RecurringTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.SellDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.Transactors;
using GrKouk.InfoSystem.Dtos.WebDtos.TransactorTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseItems;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseTransactions;
using GrKouk.WebRazor.Helpers;

namespace GrKouk.WebRazor.Automapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            #region 
            CreateMap<FinDiaryTransaction, FinDiaryExpenseTransactionDto>()
                .ForMember(dest => dest.TransactorName, opt => opt.MapFrom(src => src.Transactor.Name))
                .ForMember(dest => dest.FinTransCategoryName, opt => opt.MapFrom(src => src.FinTransCategory.Name))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.Company.Code))
                .ForMember(dest => dest.CostCentreName, opt => opt.MapFrom(src => src.CostCentre.Name))
                .ForMember(dest => dest.CostCentreCode, opt => opt.MapFrom(src => src.CostCentre.Code))
                .ForMember(dest=>dest.CompanyCurrencyId, 
                    opt=>opt.MapFrom(src=>src.Company.CurrencyId))
                .ForMember(dest => dest.AmountTotal,
                    opt => opt.MapFrom(src => src.AmountFpa + src.AmountNet));
            #endregion

            #region 
            CreateMap<FinDiaryTransaction, FinDiaryTransactionDto>()
                .ForMember(dest => dest.TransactorName, opt => opt.MapFrom(src =>
                    src.Transactor.Name
                ))
                .ForMember(dest => dest.FinTransCategoryName, opt => opt.MapFrom(src => src.FinTransCategory.Name))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.Company.Code))
                .ForMember(dest => dest.CostCentreName, opt => opt.MapFrom(src => src.CostCentre.Name))
                .ForMember(dest => dest.CostCentreCode, opt => opt.MapFrom(src => src.CostCentre.Code))
                .ForMember(dest => dest.RevenueCentreName, opt => opt.MapFrom(src => src.RevenueCentre.Name))
                .ForMember(dest => dest.RevenueCentreCode, opt => opt.MapFrom(src => src.RevenueCentre.Code))
                .ForMember(dest => dest.AmountTotal,
                    opt => opt.MapFrom(src => src.AmountFpa + src.AmountNet));
            #endregion
            CreateMap<FinDiaryTransaction, FinDiaryTransactionCreateDto>().ReverseMap();
            CreateMap<FinDiaryTransaction, FinDiaryTransactionModifyDto>().ReverseMap();
            CreateMap<FinDiaryTransaction, FinDiaryExpenceTransModifyDto>().ReverseMap();

            CreateMap<TransWarehouseDef, TransWarehouseDefListDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));

            CreateMap<WarehouseItem, WarehouseItemListDto>()
                .ForMember(dest => dest.Url, opt => opt.Ignore());
                //.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));
            CreateMap<WarehouseItem, WarehouseItemCreateDto>()
                .ForMember(dest => dest.CashRegCategoryId, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<WarehouseItem, WarehouseItemModifyDto>()
                .ForMember(dest => dest.CashRegCategoryId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<BuyDocument, BuyDocCreateAjaxNoLinesDto>().ReverseMap();
            CreateMap<BuyDocument, BuyDocModifyAjaxNoLinesDto>().ReverseMap();
            CreateMap<BuyDocModifyAjaxDto, BuyDocModifyAjaxNoLinesDto>().ReverseMap();
            CreateMap<BuyDocCreateAjaxDto, BuyDocCreateAjaxNoLinesDto>().ReverseMap();
            CreateMap<BuyDocCreateAjaxNoLinesDto, BuyDocLineAjaxDto>().ReverseMap();
            CreateMap<BuyDocument, BuyDocListDto>()
                .ForMember(dest => dest.CompanyCurrencyId,
                    opt => opt.MapFrom(src => src.Company.CurrencyId)).ReverseMap();
            CreateMap<BuyDocument , BuyDocModifyDto>().ReverseMap();
            CreateMap<BuyDocLine , BuyDocLineModifyDto>().ReverseMap();
            CreateMap<WarehouseTransaction, WarehouseTransListDto>();
            CreateMap<WarehouseTransaction, WarehouseTransCreateDto>().ReverseMap();
            CreateMap<WarehouseTransaction, WarehouseTransModifyDto>().ReverseMap();
            CreateMap<TransactorTransaction, TransactorTransCreateDto>().ReverseMap();
            CreateMap<TransactorTransaction, TransactorTransModifyDto>().ReverseMap();
            CreateMap<TransactorTransaction, TransactorTransListDto>()
                .ForMember(dest => dest.CompanyCurrencyId,
                    opt => opt.MapFrom(src => src.Company.CurrencyId))
                .ForMember(dest => dest.TransTransactorDocSeriesCode, opt => opt.MapFrom(src => src.TransTransactorDocSeries.Code))
                .ForMember(dest => dest.TransTransactorDocSeriesName, opt => opt.MapFrom(src => src.TransTransactorDocSeries.Name));
            CreateMap<TransactorTransaction, BuyDocCreateAjaxDto>().ReverseMap();
            CreateMap<TransactorTransaction, BuyDocModifyAjaxDto>().ReverseMap();
            CreateMap<TransactorTransCreateDto, BuyDocModifyAjaxDto>().ReverseMap();
            CreateMap<TransactorTransListDto, KartelaLine>()
                .ForMember(dest => dest.DocSeriesCode, opt => opt.MapFrom(src => src.TransTransactorDocSeriesCode));
            CreateMap<WarehouseItemSearchListDto, WarehouseItem>().ReverseMap();

            CreateMap<SellDocument, SellDocListDto>()
                .ForMember(dest => dest.CompanyCurrencyId,
                    opt => opt.MapFrom(src => src.Company.CurrencyId)).ReverseMap();
            CreateMap<SellDocument, SellDocCreateAjaxNoLinesDto>().ReverseMap();
            CreateMap<SellDocCreateAjaxDto, SellDocCreateAjaxNoLinesDto>().ReverseMap();
            CreateMap<SellDocument, SellDocModifyAjaxNoLinesDto>().ReverseMap();
            CreateMap<SellDocument, SellDocLineAjaxDto>().ReverseMap();
            CreateMap<SellDocument, SellDocModifyDto>().ReverseMap();
            CreateMap<SellDocLine, SellDocLineModifyDto>().ReverseMap();
            
            CreateMap<SellDocModifyAjaxDto, SellDocModifyAjaxNoLinesDto>();
            CreateMap<TransactorTransaction, SellDocCreateAjaxDto>().ReverseMap();
            CreateMap<TransactorTransaction, SellDocModifyAjaxDto>().ReverseMap();
            CreateMap<TransactorTransCreateDto, SellDocModifyAjaxDto>().ReverseMap();
            CreateMap<DiaryDto, DiaryDef>().ReverseMap();
            CreateMap<DiaryModifyDto, DiaryDef>().ReverseMap();
            CreateMap<CrCatWarehouseItem, CashRegCatProductListDto>()
                .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.ClientProfile.Company.Code));
            CreateMap<MediaEntry, MediaEntryDto>();
            CreateMap<ProductRecipe, ProductRecipeDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
          
            CreateMap<Transactor, TransactorCreateDto>().ReverseMap();
            CreateMap<Transactor, TransactorModifyDto>().ReverseMap();
            CreateMap<WrItemCode, WrItemCodeListDto>();
           
            CreateMap<RecurringTransDoc, RecurringDocCreateAjaxNoLinesDto>().ReverseMap();
            CreateMap<RecurringTransDocCreateAjaxDto, RecurringDocCreateAjaxNoLinesDto>().ReverseMap();
            CreateMap<RecurringTransDocModifyAjaxDto, RecurringTransDocModifyAjaxNoLinesDto>().ReverseMap();
            CreateMap<RecurringTransDocModifyAjaxNoLinesDto, RecurringTransDoc>().ReverseMap();
            CreateMap<RecurringTransDoc, BuyDocModifyAjaxNoLinesDto>().ReverseMap();
            CreateMap<BuyDocCreateAjaxNoLinesDto, BuyDocLineAjaxDto>().ReverseMap();
            CreateMap<RecurringTransDoc, RecurringDocModifyDto>()
                .ForMember(dest => dest.BuyDocLines,
                    opt => opt.MapFrom(src => src.DocLines));

            
            CreateMap<RecurringTransDoc, RecurringDocListDto>()
                .ForMember(dest => dest.CompanyCurrencyId,
                    opt => opt.MapFrom(src => src.Company.CurrencyId)).ReverseMap();
            CreateMap<BuyDocument, RecurringDocModifyDto>()
                .ForMember(dest => dest.DocSeriesId,
                    opt => opt.MapFrom(src => src.BuyDocSeriesId))
                .ReverseMap();
            CreateMap<BuyDocLine, RecurringDocLineModifyDto>().ReverseMap();
            CreateMap<SellDocument, RecurringDocModifyDto>()
               .ForMember(dest => dest.DocSeriesId,
                   opt => opt.MapFrom(src => src.SellDocSeriesId))
               .ForMember(dest=>dest.BuyDocLines,
               opt=>opt.MapFrom(src=>src.SellDocLines))
               .ReverseMap();
            CreateMap<SellDocLine, RecurringDocLineModifyDto>();
            CreateMap<RecurringTransDocLine, RecurringDocLineModifyDto>();
            CreateMap<RecurringTransDoc, BuyDocCreateAjaxDto>();
            CreateMap<RecurringTransDocLine, BuyDocLineAjaxDto>();
            CreateMap<RecurringTransDoc, SellDocCreateAjaxDto>()
                .ForMember(dest => dest.SellDocSeriesId,
                    opt => opt.MapFrom(src => src.DocSeriesId))
                .ForMember(dest => dest.SellDocLines,
                    opt => opt.MapFrom(src => src.DocLines));
            CreateMap<RecurringTransDocLine, SellDocLineAjaxDto>()
                .ForMember(d => d.Q1,
                    opt => opt.MapFrom(src => src.Quontity1))
                .ForMember(d => d.Q2,
                    opt => opt.MapFrom(src => src.Quontity2))
                .ForMember(d => d.MainUnitId,
                    opt => opt.MapFrom(src => src.PrimaryUnitId))
                .ForMember(d => d.SecUnitId,
                    opt => opt.MapFrom(src => src.SecondaryUnitId))
                .ForMember(d => d.Price,
                    opt => opt.MapFrom(src => src.UnitPrice));



        }
    }
}
