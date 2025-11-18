using AutoMapper.Internal;
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

        // Auto mapper
        services.AddAutoMapper(cfg =>
        {
            cfg.Internal().MethodMappingEnabled = false;
        }, typeof(IdentityMappingProfile).Assembly);

        return services;
    }
}
