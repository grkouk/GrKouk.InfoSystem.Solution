using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using GrKouk.Web.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrKouk.WebApi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GrKouk.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApiDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")))
                .AddDbContext<SecurityDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<SecurityDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<SecurityDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            //Configure Automapper
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<FinDiaryTransaction, FinDiaryTransactionDto>()
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

                cfg.CreateMap<FinDiaryTransaction, FinDiaryExpenseTransactionDto>()
                    .ForMember(dest => dest.TransactorName, opt => opt.MapFrom(src =>src.Transactor.Name))
                    .ForMember(dest => dest.FinTransCategoryName, opt => opt.MapFrom(src => src.FinTransCategory.Name))
                    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                    .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.Company.Code))
                    .ForMember(dest => dest.CostCentreName, opt => opt.MapFrom(src => src.CostCentre.Name))
                    .ForMember(dest => dest.CostCentreCode, opt => opt.MapFrom(src => src.CostCentre.Code))
                    .ForMember(dest => dest.AmountTotal,
                        opt => opt.MapFrom(src => src.AmountFpa + src.AmountNet));

                cfg.CreateMap<FinDiaryTransaction, FinDiaryTransactionCreateDto>().ReverseMap();
                cfg.CreateMap<FinDiaryTransaction, FinDiaryTransactionModifyDto>().ReverseMap();
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
