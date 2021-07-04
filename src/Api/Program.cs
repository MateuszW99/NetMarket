using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Api.Common;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
            
            try
            {
                Log.Information("Starting up");
                var context = services.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
                
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                if (!await roleManager.RoleExistsAsync(Roles.User))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>()
                    {
                        Name = Roles.User,
                        NormalizedName = Roles.User.ToUpper()
                    });
                }
                
                if (!await roleManager.RoleExistsAsync(Roles.Supervisor))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>()
                    {
                        Name = Roles.Supervisor,
                        NormalizedName = Roles.Supervisor.ToUpper()
                    });
                }
                
                if (!await roleManager.RoleExistsAsync(Roles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>()
                    {
                        Name = Roles.Admin,
                        NormalizedName = Roles.Admin.ToUpper()
                    });
                }

                if (!await context.Items.AnyAsync())
                {
                    var itemSeeder = services.GetRequiredService<ISeeder<List<Item>>>();
                    var items = await itemSeeder.Seed();
                    await context.Items.AddRangeAsync(items);
                    await context.SaveChangesAsync(CancellationToken.None);
                }
                

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
