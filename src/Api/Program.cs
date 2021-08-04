using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Api.Common;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
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
            Log.Information("Starting up");
            
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
                
                var roleSeeder = services.GetRequiredService<RoleSeeder>();
                await roleSeeder.SeedAsync();

                var userSeeder = services.GetRequiredService<UserSeeder>();
                await userSeeder.SeedAsync();
                
                if (!await context.Items.AnyAsync())
                {
                    var itemSeeder = services.GetRequiredService<ISeeder<List<Item>>>();
                    var items = await itemSeeder.SeedAsync();
                    await context.Items.AddRangeAsync(items);
                    await context.SaveChangesAsync(CancellationToken.None);
                }

                if (!await context.Sizes.AnyAsync())
                {
                    var sizeSeeder = services.GetRequiredService<ISeeder<List<Size>>>();
                    var sizes = await sizeSeeder.SeedAsync();
                    await context.Sizes.AddRangeAsync(sizes);
                    await context.SaveChangesAsync(CancellationToken.None);
                }
                
                await host.RunAsync();
                Log.Information("Api running");
                
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}