using System.Reflection;
using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(assembly);
            services.AddValidatorsFromAssembly(assembly);
            services.AddAutoMapper(assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IUserSettingsService, UserSettingsService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IAskService, AskService>();
            services.AddTransient<IBidService, BidService>();
            services.AddTransient<IFeeService, FeeService>();
            
            return services;
        }
    }
}