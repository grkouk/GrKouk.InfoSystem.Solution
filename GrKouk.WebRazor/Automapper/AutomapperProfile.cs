using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.MediaEntities;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using GrKouk.InfoSystem.Dtos.WebDtos;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyDocuments;
using GrKouk.InfoSystem.Dtos.WebDtos.CashRegister;
using GrKouk.InfoSystem.Dtos.WebDtos.CustomerTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.Diaries;
using GrKouk.InfoSystem.Dtos.WebDtos.Media;
using GrKouk.InfoSystem.Dtos.WebDtos.ProductRecipies;
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

            CreateMap<WarehouseItem, WarehouseItemListDto>();
                //.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));
            CreateMap<WarehouseItem, WarehouseItemCreateDto>().ReverseMap();
            CreateMap<WarehouseItem, WarehouseItemModifyDto>().ReverseMap();

            CreateMap<BuyDocument, BuyDocCreateAjaxNoLinesDto>().ReverseMap();
            CreateMap<BuyDocument, BuyDocModifyAjaxNoLinesDto>().ReverseMap();
            CreateMap<BuyDocCreateAjaxNoLinesDto, BuyDocLineAjaxDto>().ReverseMap();
            CreateMap<BuyDocument, BuyDocListDto>().ReverseMap();

            CreateMap<WarehouseTransaction, WarehouseTransListDto>();
            CreateMap<WarehouseTransaction, WarehouseTransCreateDto>().ReverseMap();
            CreateMap<WarehouseTransaction, WarehouseTransModifyDto>().ReverseMap();
            CreateMap<TransactorTransaction, TransactorTransCreateDto>().ReverseMap();
            CreateMap<TransactorTransaction, TransactorTransModifyDto>().ReverseMap();
            CreateMap<TransactorTransaction, TransactorTransListDto>()
                .ForMember(dest => dest.TransTransactorDocSeriesCode, opt => opt.MapFrom(src => src.TransTransactorDocSeries.Code))
                .ForMember(dest => dest.TransTransactorDocSeriesName, opt => opt.MapFrom(src => src.TransTransactorDocSeries.Name));
            CreateMap<TransactorTransaction, BuyDocCreateAjaxDto>().ReverseMap();
            CreateMap<TransactorTransaction, BuyDocModifyAjaxDto>().ReverseMap();
            CreateMap<TransactorTransCreateDto, BuyDocModifyAjaxDto>().ReverseMap();
            CreateMap<TransactorTransListDto, KartelaLine>()
                .ForMember(dest => dest.DocSeriesCode, opt => opt.MapFrom(src => src.TransTransactorDocSeriesCode));
            CreateMap<WarehouseItemSearchListDto, WarehouseItem>();

            CreateMap<SellDocument, SellDocListDto>().ReverseMap();
            CreateMap<SellDocument, SellDocCreateAjaxNoLinesDto>().ReverseMap();
            CreateMap<SellDocument, SellDocModifyAjaxNoLinesDto>().ReverseMap();
            CreateMap<SellDocument, SellDocLineAjaxDto>().ReverseMap();
            CreateMap<SellDocument, SellDocListDto>().ReverseMap();
            CreateMap<SellDocLine, SellDocLineModifyDto>().ReverseMap();

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
            //CreateMap<BuyDocTypeDef, DiaryDocTypeItem>()
            //    .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));
            //CreateMap<SellDocTypeDef, DiaryDocTypeItem>()
            //    .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));
        }
    }
}
