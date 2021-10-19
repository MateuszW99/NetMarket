using System.Collections.Generic;
using System.Text;
using Api.Services;
using Application.Common.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Api.Common
{
    public static class ServiceInstallers
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1", 
                    new OpenApiInfo
                    {
                        Title = "NetMarket API",
                        Version = "v1"
                    });
                
                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme()
                    {
                        Description   = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        }, 
                        new List<string>()
                    }
                });
            });
            
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(JwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddTransient<IIdentityService, IdentityService>();
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.SaveToken = true;
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =  new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = true,
                        ValidateLifetime = true
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.UserAccess, 
                    policy => policy.RequireAssertion(context => 
                        context.User.IsInRole(Roles.User)));  
                
                options.AddPolicy(Policies.SupervisorAccess, 
                    policy => policy.RequireAssertion(context =>
                        context.User.IsInRole(Roles.Supervisor) ||
                        context.User.IsInRole(Roles.Admin)));
                
                options.AddPolicy(Policies.AdminAccess, 
                    policy => policy.RequireAssertion(context =>
                        context.User.IsInRole(Roles.Admin)));
            });
            
            return services;
        }

        public static IServiceCollection AddInternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IHttpService, HttpService>();
            services.AddTransient<IUserManagerService, UserManagerService>();
            return services;
        }
    }
}