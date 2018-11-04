using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.Web.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GrKouk.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            
            //This doesn't work.
            //TODO Change this back to the default
            //var a =CreateWebHostBuilder(args).Build();
            //var services = a.Services;
            //try
            //{
            //    var serviceProvider = services.GetRequiredService<IServiceProvider>();
            //    var configuration = services.GetRequiredService<IConfiguration>();
            //    Seed.CreateRoles(serviceProvider, configuration).Wait();

            //}
            //catch (Exception exception)
            //{
            //    var logger = services.GetRequiredService<ILogger<Program>>();
            //    logger.LogError(exception, "An error occurred while creating roles");
            //}
            //a.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        
    }
}
