using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;

namespace GrKouk.WebRazor.Automapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<FinDiaryTransaction, FinDiaryExpenseTransactionDto>()
                .ForMember(dest => dest.TransactorName, opt => opt.MapFrom(src => src.Transactor.Name))
                .ForMember(dest => dest.FinTransCategoryName, opt => opt.MapFrom(src => src.FinTransCategory.Name))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.Company.Code))
                .ForMember(dest => dest.CostCentreName, opt => opt.MapFrom(src => src.CostCentre.Name))
                .ForMember(dest => dest.CostCentreCode, opt => opt.MapFrom(src => src.CostCentre.Code))
                .ForMember(dest => dest.AmountTotal,
                    opt => opt.MapFrom(src => src.AmountFpa + src.AmountNet));
        }
    }
}
