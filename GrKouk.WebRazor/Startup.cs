using System;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using GrKouk.WebApi.Data;
using GrKouk.WebRazor.Automapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrKouk.WebRazor.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NToastNotify;

namespace GrKouk.WebRazor
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
            #region CommentOut cors
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAllOrigins",
            //        builder =>
            //        {
            //            builder.AllowAnyOrigin()
            //                .AllowAnyMethod()
            //                .AllowAnyHeader()
            //                .AllowCredentials()
            //                .WithExposedHeaders("X-Pagination");
            //        });
            //    //options.AddPolicy("AllowSpecificOrigins",
            //    //    builder =>
            //    //    {
            //    //        builder.WithOrigins( "http://potos.tours",
            //    //            "http://thassos-rent-a-bike.com").AllowAnyMethod().AllowAnyHeader();
            //    //    });
            //});
#endregion
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

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<SecurityDbContext>();


            services.AddLocalization(o =>
            {
                // We will put our translations in a folder called Resources
                o.ResourcesPath = "Resources";
            });
            services.AddDistributedMemoryCache();
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); });
            services.AddMvc()
                .AddNToastNotifyToastr(new ToastrOptions()
                {
                    ProgressBar = true,
                    PositionClass = ToastPositions.BottomRight,
                    TimeOut = 5000,
                    ExtendedTimeOut = 1000
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAutoMapper(typeof(Startup));
            //services.AddAutoMapper(cfg =>
            //{
            //    cfg.AddProfile(new AutoMapperProfile());

            //});
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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            IList<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("el-GR"),
                new CultureInfo("en-US")

            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("el-GR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            app.UseNToastNotify();
            app.UseSession();
           // app.UseCors("AllowAllOrigins");
            app.UseMvc();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjUzODczQDMxMzgyZTMxMmUzMG5UNHArR2l3MlJYaklGMlI5aTJXVnRuNnJuN1JLQVk5V1lUV0xaTDVxeGM9;MjUzODc0QDMxMzgyZTMxMmUzMFd6SVAveHB3cVZBQ0RsMUM3MXlhL0JKWjlUbU00R1ladTRlN2RsbGJXWmM9;MjUzODc1QDMxMzgyZTMxMmUzMGVIR01rLzlHYWsvUVlIYXJoSzF5RWQ3dnBxdEtmYjB2YTZRVHROQ0VnRlE9;MjUzODc2QDMxMzgyZTMxMmUzMGJ3aEw0RTBFWDBLUjFxcWIzQk0yRlNEaFdpWExYc2s4aWJIbmFxeE1aNW89;MjUzODc3QDMxMzgyZTMxMmUzMFBiSjkzSHVqMVVhRUNETmE0V1hTZG9PTTF5NDBQYTJicUVXQ2V0WXJRVlE9;MjUzODc4QDMxMzgyZTMxMmUzMGVPS0JHRmlTaTVYRm1lVWdsdTd0YnhuaHVhUnIvblM1OGt4R3g3N0o5Q289;MjUzODc5QDMxMzgyZTMxMmUzMGJuS3REbjJwV3haSWpyb29wT0ZzL2U2bXJmZ0NDaWs2R20xNERtNHVqQ1U9;MjUzODgwQDMxMzgyZTMxMmUzMEJLc2NUWGZjUC84Yms3TDJFdm9iY1BCeTJESDlYVkZvbDVsazFmUTlEYXc9;MjUzODgxQDMxMzgyZTMxMmUzMG15d2c3dXUwZEdEakhyUlMyZ0FYdGRPWHZKM05aNHdXeEN6amVVdU1ralU9;NT8mJyc2IWhia31ifWN9Z2FoYmF8YGJ8ampqanNiYmlmamlmanMDHmg0Njg8JjgTOzwnPjI6P30wPD4=;MjUzODgyQDMxMzgyZTMxMmUzMEJmeXlUUlFUelg0Wnpsa1VCS0FZaDllQkJoSzF4TWp4NVRDbFZIZDNTTWM9");
        }
    }
}
