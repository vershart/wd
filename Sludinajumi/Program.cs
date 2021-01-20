using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sludinajumi.Api.Models;
using Sludinajumi.Api.Models.Data;
using Sludinajumi.Api.Data;
using System.Net;

namespace Sludinajumi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SludinajumiContext>();
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                    context.SaveChanges();

                    InitialData initialData = new InitialData();

                    if (!context.Languages.Any())
                        context.Languages.AddRange(initialData.GenerateLanguages());
                    context.SaveChanges();

                    if (!context.Categories.Any()) {
                        foreach (Category cat in initialData.GenerateCategories()) {
                            context.Categories.Add(cat);
                            context.SaveChanges();
                        }
                    }

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
            
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseSetting("detailedErrors", "true")
				.UseIISIntegration()
				.UseStartup<Startup>()
				.CaptureStartupErrors(true)
				.Build();
	}
}
