using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FocusLifePlus.Infrastructure.Persistence;
using FocusLifePlus.Infrastructure.Persistence.Repositories;
using FocusLifePlus.Domain.Repositories;

namespace FocusLifePlus.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IFocusTaskRepository, FocusTaskRepository>();

            return services;
        }
    }
} 