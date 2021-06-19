using System;
using System.Threading.Tasks;
using Api.Common;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                await context.Database.EnsureCreatedAsync();
                
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

            }
            catch (Exception ex)
            {
                // TODO: add logs
            }
            
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
