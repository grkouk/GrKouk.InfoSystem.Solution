using AutoMapper;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;

namespace GrKouk.WebApi.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
           

            CreateMap<FinDiaryTransaction, FinDiaryTransactionListDto>()
                .ForMember(dest => dest.TransactorName, opt => opt.MapFrom(src =>src.Transactor.Name))
                .ForMember(dest => dest.FinTransCategoryName, opt => opt.MapFrom(src => src.FinTransCategory.Name))
                .ForMember(dest => dest.RevenueCentreCode, opt => opt.MapFrom(src => src.RevenueCentre.Code))
                .ForMember(dest => dest.AmountTotal,
                    opt => opt.MapFrom(src => src.AmountFpa + src.AmountNet));

                CreateMap<FinDiaryTransaction, FinDiaryTransactionDto>()
                    .ForMember(dest => dest.TransactorName, opt => opt.MapFrom(src =>
                        src.Transactor.Name
                    ))
                    .ForMember(dest => dest.FinTransCategoryName, opt => opt.MapFrom(src => src.FinTransCategory.Name))
                    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                    .ForMember(dest => dest.CostCentreName, opt => opt.MapFrom(src => src.CostCentre.Name))
                    .ForMember(dest => dest.RevenueCentreName, opt => opt.MapFrom(src => src.RevenueCentre.Name))
                    .ForMember(dest => dest.AmountTotal,
                        opt => opt.MapFrom(src => src.AmountFpa + src.AmountNet));


            CreateMap<FinDiaryTransaction, FinDiaryTransactionCreateDto>().ReverseMap();
            CreateMap<FinDiaryTransaction, FinDiaryTransactionModifyDto>().ReverseMap();
            CreateMap<FinDiaryTransaction, FinDiaryExpenceTransModifyDto>().ReverseMap();
        }
    }
}
