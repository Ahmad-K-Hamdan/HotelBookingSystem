using HotelBookingSystem.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using HotelBookingSystem.Infrastructure.Identity.Services;
using HotelBookingSystem.Infrastructure.Services;

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

        // Bind JWT
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Services 
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
