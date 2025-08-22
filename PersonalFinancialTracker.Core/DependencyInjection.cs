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
   
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            // Register AutoMapper - scan the entire assembly for all Profile classes
            services.AddAutoMapper(typeof(TransactionAddRequestToTransactionMappingProfile).Assembly);

            // Register FluentValidation validators
            services.AddValidatorsFromAssemblyContaining<TransactionAddRequestValidator>();

            // Register core services
            services.AddScoped<IFinancialTransactionServices, TransactionServices>();

            return services;
        }
    }
