using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using GrKouk.WebApi.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GrKouk.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApiDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Configure Automapper
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<FinDiaryTransaction, FinDiaryTransactionDto>()
                    .ForMember(dest => dest.TransactorName, opt => opt.MapFrom(src =>
                        src.Transactor.Name
                    ))
                    .ForMember(dest => dest.FinTransCategoryName, opt => opt.MapFrom(src => src.FinTransCategory.Name))
                    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                    .ForMember(dest => dest.CostCentreName, opt => opt.MapFrom(src => src.CostCentre.Name))
                    .ForMember(dest => dest.RevenueCentreName, opt => opt.MapFrom(src => src.RevenueCentre.Name))
                    .ForMember(dest => dest.AmountTotal,
                        opt => opt.ResolveUsing(src => src.AmountFpa + src.AmountNet));
                cfg.CreateMap<FinDiaryTransaction, FinDiaryTransactionCreateDto>().ReverseMap();
                cfg.CreateMap<FinDiaryTransaction, FinDiaryTransactionModifyDto>().ReverseMap();
            });
            app.UseMvc();
        }
    }
}
