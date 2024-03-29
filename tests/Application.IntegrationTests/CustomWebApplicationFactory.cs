﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Api;
using Application.Common.Interfaces;
using Application.IntegrationTests.Helpers;
using Application.Models.DTOs;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public CustomWebApplicationFactory()
        {
            _databaseName = Guid.NewGuid().ToString();
        }

        private readonly string _databaseName;
        
        public string CurrentUserId { get; set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //Replace HttpService
                var httpServiceDescriptor = services.FirstOrDefault(d =>
                    d.ServiceType == typeof(IHttpService));

                services.Remove(httpServiceDescriptor);
                services.AddTransient(_ =>
                    Mock.Of<IHttpService>(x => x.GetUserId() == CurrentUserId));


                //Replace ApplicationDbContext
                var applicationDbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<ApplicationDbContext>));

                services.Remove(applicationDbContextDescriptor);
                services.AddDbContext<ApplicationDbContext>(options => { options.UseInMemoryDatabase(_databaseName); });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var roleSeeder = scopedServices.GetRequiredService<RoleSeeder>();
                    
                    var userSeeder = scopedServices.GetRequiredService<UserSeeder>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();
                    
                    context.Database.EnsureCreatedAsync().Wait();

                    try
                    {
                        roleSeeder.SeedAsync().Wait();
                        userSeeder.SeedAsync().Wait();
                        
                        if (!context.Items.Any())
                        {
                            var itemSeeder = scopedServices.GetRequiredService<ISeeder<List<Item>>>();
                            var items = itemSeeder.SeedAsync().Result;
                            context.Items.AddRangeAsync(items).Wait();
                            context.SaveChangesAsync(CancellationToken.None).Wait();
                        }

                        if (!context.Sizes.Any())
                        {
                            var sizeSeeder = scopedServices.GetRequiredService<ISeeder<List<Size>>>();
                            var sizes = sizeSeeder.SeedAsync().Result;
                            context.Sizes.AddRange(sizes);
                            context.SaveChangesAsync(CancellationToken.None).Wait();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the test database" +
                                            "Error: {Message}", ex.Message);
                    }
                    
                }
            });
        }
    }
}