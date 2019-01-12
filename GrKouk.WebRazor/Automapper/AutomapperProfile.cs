using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.FinConfig;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using GrKouk.InfoSystem.Dtos.WebDtos;
using GrKouk.InfoSystem.Dtos.WebDtos.BuyMaterialsDocs;
using GrKouk.InfoSystem.Dtos.WebDtos.CustomerTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.Materials;
using GrKouk.InfoSystem.Dtos.WebDtos.SupplierTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.TransactorTransactions;
using GrKouk.InfoSystem.Dtos.WebDtos.WarehouseTransactions;

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

            CreateMap<SupplierTransaction, SupplierTransactionCreateDto>()
                .ReverseMap();
            CreateMap<SupplierTransaction, SupplierTransactionModifyDto>()
                .ReverseMap();

            CreateMap<SupplierTransaction, SupplierTransactionListDto>()
                .ForMember(dest=>dest.TransSupplierSeriesCode,opt=>opt.MapFrom(src=>src.TransSupplierDocSeries.Code))
                .ForMember(dest => dest.TransSupplierSeriesName, opt => opt.MapFrom(src => src.TransSupplierDocSeries.Name));

            CreateMap<Material, MaterialListDto>();
                //.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));
            CreateMap<Material, MaterialCreateDto>().ReverseMap();
            CreateMap<Material, MaterialModifyDto>().ReverseMap();
            CreateMap<BuyMaterialsDocument, BuyMaterialsDocCreateAjaxNoLinesDto>().ReverseMap();
            CreateMap<BuyMaterialsDocument, BuyMaterialsDocModifyAjaxNoLinesDto>().ReverseMap();
            CreateMap<BuyMaterialsDocCreateAjaxNoLinesDto, BuyMaterialDocLineAjaxDto>().ReverseMap();
            CreateMap<BuyMaterialsDocument, BuyMaterialsDocListDto>().ReverseMap();
            CreateMap<SupplierTransaction, BuyMaterialsDocCreateAjaxDto>().ReverseMap();
            CreateMap<SupplierTransaction, BuyMaterialsDocModifyAjaxDto>().ReverseMap();
            CreateMap<SupplierTransactionCreateDto, BuyMaterialsDocModifyAjaxDto>().ReverseMap();
            CreateMap<WarehouseTransaction, WarehouseTransListDto>();
            CreateMap<WarehouseTransaction, WarehouseTransCreateDto>().ReverseMap();
            CreateMap<WarehouseTransaction, WarehouseTransModifyDto>().ReverseMap();
            CreateMap<CustomerTransaction, CustomerTransactionCreateDto>()
                .ReverseMap();
            CreateMap<CustomerTransaction, CustomerTransactionModifyDto>()
                .ReverseMap();
            CreateMap<CustomerTransaction, CustomerTransactionListDto>()
                .ForMember(dest => dest.TransCustomerSeriesCode, opt => opt.MapFrom(src => src.TransCustomerDocSeries.Code))
                .ForMember(dest => dest.TransCustomerSeriesName, opt => opt.MapFrom(src => src.TransCustomerDocSeries.Name));
            CreateMap<TransactorTransaction, TransactorTransCreateDto>()
                .ReverseMap();
            CreateMap<TransactorTransaction, TransactorTransModifyDto>()
                .ReverseMap();
            CreateMap<TransactorTransaction, TransactorTransListDto>()
                .ForMember(dest => dest.TransTransactorDocSeriesCode, opt => opt.MapFrom(src => src.TransTransactorDocSeries.Code))
                .ForMember(dest => dest.TransTransactorDocSeriesName, opt => opt.MapFrom(src => src.TransTransactorDocSeries.Name));
            //CreateMap<WarehouseTransaction, BuyMaterialsDocCreateAjaxDto>().ReverseMap();
            CreateMap<TransactorTransaction, BuyMaterialsDocCreateAjaxDto>().ReverseMap();
            CreateMap<TransactorTransaction, BuyMaterialsDocModifyAjaxDto>().ReverseMap();
            CreateMap<TransactorTransCreateDto, BuyMaterialsDocModifyAjaxDto>().ReverseMap();
        }
    }
}
