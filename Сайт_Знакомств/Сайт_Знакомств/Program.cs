using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Сайт_Знакомств.Data;
using Сайт_Знакомств.Models;

namespace Сайт_Знакомств
{
    public class Program
    {
        public static object DbInitializator { get; private set; }

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var serives = scope.ServiceProvider;
                try
                {
                    var context = serives.GetRequiredService<ApplicationDbContext>();
                    var userManger = serives.GetRequiredService<UserManager<User>>();
                    await context.InitialContextAsync(userManger);
                }
                catch (Exception ex)
                {
                    var logger = serives.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Error during db seed");
                }
            }


            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
