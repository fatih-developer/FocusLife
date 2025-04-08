using System.Text;
using AspNetCoreRateLimit;
using FocusLifePlus.Application.Common.Middleware;
using FocusLifePlus.Application.Common.DependencyInjection;
using FocusLifePlus.Infrastructure;
using FocusLifePlus.Infrastructure.Authentication;
using FocusLifePlus.API.Filters;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

try
{
    Log.Information("Starting web host");
    var builder = WebApplication.CreateBuilder(args);

    // Serilog configuration
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    try
    {
        // Add Infrastructure Services first
        builder.Services.AddInfrastructureServices(builder.Configuration);
    }
    catch (Exception ex)
    {
        Log.Error($"mesaj:{ex.Message} inner mesaj:{ex.InnerException.Message}");
    }
    
    
    // Then add Application Services
    builder.Services.AddApplication();

    // Rate Limiting
    builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
    builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
    builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
    builder.Services.AddMemoryCache();

    builder.Services.AddControllers();

    // Add HttpContextAccessor
    builder.Services.AddHttpContextAccessor();

    // JWT Configuration
    var jwtSection = builder.Configuration.GetSection("Jwt");
    builder.Services.Configure<JwtOptions>(jwtSection);

    var jwtOptions = jwtSection.Get<JwtOptions>();
    if (jwtOptions == null)
    {
        throw new InvalidOperationException("JWT configuration is missing in appsettings.json");
    }

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };
    });

    // Swagger/OpenAPI konfigürasyonu
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "FocusLife+ API",
            Version = "v1",
            Description = "FocusLife+ uygulaması için REST API",
            Contact = new OpenApiContact
            {
                Name = "FocusLife+ Team",
                Email = "contact@focuslifeplus.com"
            }
        });

        // JWT Bearer Authentication için Swagger UI konfigürasyonu
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        // XML comments için
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

        // Swagger UI'da model örnek değerlerini göster
        c.SchemaFilter<SwaggerExampleSchemaFilter>();
    });

    // Role-based Authorization Policies
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
        options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
        options.AddPolicy("RequireAdminOrUserRole", policy => policy.RequireRole("Admin", "User"));
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "FocusLife+ API V1");
            c.RoutePrefix = string.Empty; // Ana sayfada Swagger UI'ı göster
        });
    }

    app.UseHttpsRedirection();

    // Serilog request logging
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    });

    // Rate Limiting
    app.UseIpRateLimiting();

    // Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // Add Exception Handling Middleware
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.MapControllers();

    app.Run();
    Log.Information("Application started successfully");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw; // Yeniden fırlat ki process doğru exit code ile kapansın
}
finally
{
    Log.Information("Shutting down application");
    Log.CloseAndFlush();
} 