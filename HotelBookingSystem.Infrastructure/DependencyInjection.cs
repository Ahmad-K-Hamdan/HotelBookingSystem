using AutoMapper.Internal;
using HotelBookingSystem.Application.Common.Exceptions.Handlers;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using HotelBookingSystem.Infrastructure.Data;
using HotelBookingSystem.Infrastructure.Identity.JwtTokens;
using HotelBookingSystem.Infrastructure.Identity.Mapping;
using HotelBookingSystem.Infrastructure.Identity.Models;
using HotelBookingSystem.Infrastructure.Identity.Services;
using HotelBookingSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;

namespace HotelBookingSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        );

        // SendGrid Service
        AddSendGridService(services, configuration);

        // ASP.NET Identity
        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // JWT Settings Binding + Token Generator
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        // Services 
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IEmailService, EmailService>();

        // Handlers
        services.AddTransient<IExceptionHandler, ValidationExceptionHandler>();
        services.AddTransient<IExceptionHandler, IdentityExceptionHandler>();
        services.AddTransient<IExceptionHandler, DefaultExceptionHandler>();

        // Auto mapper
        services.AddAutoMapper(cfg =>
        {
            cfg.Internal().MethodMappingEnabled = false;
        }, typeof(IdentityMappingProfile).Assembly);

        return services;
    }

    private static IServiceCollection AddSendGridService(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind SendGridSettings
        var sendGridSection = configuration.GetSection("SendGridSettings");
        services.Configure<SendGridSettings>(sendGridSection);

        var sendGridSettings = sendGridSection.Get<SendGridSettings>();

        // Validate config
        if (string.IsNullOrWhiteSpace(sendGridSettings?.ApiKey))
        {
            throw new InvalidOperationException("SendGrid API Key is missing. Please set SendGridSettings:ApiKey in appsettings.json");
        }

        if (string.IsNullOrWhiteSpace(sendGridSettings.FromEmail))
        {
            throw new InvalidOperationException("SendGrid 'FromEmail' is missing in SendGridSettings.");
        }

        if (string.IsNullOrWhiteSpace(sendGridSettings.FromName))
        {
            throw new InvalidOperationException("SendGrid 'FromName' is missing in SendGridSettings.");
        }

        // Register SendGrid Client
        services.AddSingleton<ISendGridClient>(_ => new SendGridClient(sendGridSettings.ApiKey));

        // Register EmailService
        services.AddScoped<IEmailService>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<SendGridSettings>>();
            var logger = sp.GetRequiredService<ILogger<EmailService>>();
            var client = sp.GetRequiredService<ISendGridClient>();

            return new EmailService(opts, logger, client);
        });

        return services;
    }
}
