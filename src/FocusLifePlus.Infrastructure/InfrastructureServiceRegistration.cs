using System.Text;
using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Domain.Repositories;
using FocusLifePlus.Infrastructure.Authentication;
using FocusLifePlus.Infrastructure.Persistence;
using FocusLifePlus.Infrastructure.Persistence.Repositories;
using FocusLifePlus.Infrastructure.Services;
using FocusLifePlus.Application.Interfaces.Services;
using FocusLifePlus.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FocusLifePlus.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext Configuration
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), 
            ServiceLifetime.Scoped);

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // Repository Registrations
        services.AddScoped<Domain.Repositories.IFocusTaskRepository, FocusTaskRepository>();
        services.AddScoped<IFocusTagRepository, FocusTagRepository>();
        services.AddScoped<IFocusCategoryRepository, FocusCategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Service Registrations
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordValidationService, PasswordValidationService>();

        //// JWT Configuration
        //var jwtSection = configuration.GetSection(JwtOptions.SectionName);
        //var jwtOptions = jwtSection.Get<JwtOptions>();
        //if (jwtOptions == null)
        //{
        //    throw new InvalidOperationException("JWT configuration is missing in appsettings.json");
        //}
        //services.Configure<JwtOptions>(jwtSection);

        //services.AddAuthentication(options =>
        //{
        //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //})
        //.AddJwtBearer(options =>
        //{
        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        ClockSkew = TimeSpan.Zero,
        //        ValidIssuer = jwtOptions.Issuer,
        //        ValidAudience = jwtOptions.Audience,
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        //    };
        //});

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
} 