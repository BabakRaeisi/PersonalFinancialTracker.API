
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinancialTracker.Core.Mappers;
using PersonalFinancialTracker.Core.ServiceContracts;
using PersonalFinancialTracker.Core.Services;
using PersonalFinancialTracker.Core.Validators;

namespace PersonalFinancialTracker.Core; 
 
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services,IConfiguration configuration)
        {
        // Register your core services here
        // For example, if you have a service for handling transactions, register it
        // Example:
        // services.AddScoped<ITransactionService, TransactionService>();
        services.AddAutoMapper(typeof(TransactionUpdateRequestToProductMappingProfile).Assembly);
        services.AddValidatorsFromAssemblyContaining<TransactionAddRequestValidator>();
        services.AddScoped<IFinancialTransactionServices, TransactionServices>();
        return services;
        }
    }
 
