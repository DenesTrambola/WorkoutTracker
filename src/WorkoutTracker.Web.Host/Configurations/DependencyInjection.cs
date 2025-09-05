namespace WorkoutTracker.Web.Host.Configurations;

using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Scrutor;
using Serilog;
using WorkoutTracker.Application;
using WorkoutTracker.Application.Shared.Primitives;
using WorkoutTracker.Application.Users.Primitives;
using WorkoutTracker.Domain.Measurements.Enums;
using WorkoutTracker.Infrastructure;
using WorkoutTracker.Infrastructure.Models;
using WorkoutTracker.Infrastructure.Services;
using WorkoutTracker.Persistence;
using WorkoutTracker.Web.Presentation;

internal static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(ApplicationAssemblyReference.Assembly));

        return services;
    }

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        ConfigurationManager configurationManager,
        string environmentName)
    {
        services.Scan(selector => selector
        .FromAssemblies(InfrastructureAssemblyReference.Assembly)
        .AddClasses(false)
        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        .AsMatchingInterface()
        .WithScopedLifetime());

        configurationManager
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        var smtpOptions = configuration.GetSection("Smtp").Get<SmtpEmailOptions>()!;
        services.AddSingleton(smtpOptions);

        services.AddScoped<IAccessTokenProvider, JwtTokenProvider>();
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IPasswordHasher, PasswordHasherService>();

        return services;
    }

    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Scan(selector => selector
        .FromAssemblies(PersistenceAssemblyReference.Assembly)
        .AddClasses(false)
        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        .AsMatchingInterface()
        .WithScopedLifetime());

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Database")));

        return services;
    }

    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        ConfigureHostBuilder host,
        ConfigureWebHostBuilder webHost)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();
        host.UseSerilog();

        services
            .AddControllers()
            .AddApplicationPart(WebPresentationAssemblyReference.Assembly)
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            c.MapType<MeasurementUnit>(() =>
                new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetNames<MeasurementUnit>()
                        .Select(name => new OpenApiString(name))
                        .Cast<IOpenApiAny>()
                        .ToList()
                });
        });

        webHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(1344);
        });

        services.AddEndpointsApiExplorer();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy
                => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        return services;
    }

    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtOptions>()!;
        services.AddSingleton(jwtSettings);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "JwtBearer";
            options.DefaultChallengeScheme = "JwtBearer";
        })
        .AddJwtBearer("JwtBearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });

        services.AddAuthorization();

        return services;
    }
}
