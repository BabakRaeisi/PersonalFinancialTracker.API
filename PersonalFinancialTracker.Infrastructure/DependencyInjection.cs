using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PersonalFinancialTracker.Infrastructure.Context;
using PersonalFinancialTracker.Core.RepositoryContracts;
using PersonalFinancialTracker.Infrastructure.RepositoryServices;



namespace PersonalFinancialTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Register repositories or other infrastructure services if needed
            services.AddScoped<IRepositoryServices, TransactionRepository>(); // Add this line

            return services;
        }
    }
}
