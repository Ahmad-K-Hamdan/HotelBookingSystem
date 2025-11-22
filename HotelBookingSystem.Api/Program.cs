
using HotelBookingSystem.Api.Middleware;
using HotelBookingSystem.Application;
using HotelBookingSystem.Application.Common.Exceptions.Handlers;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using HotelBookingSystem.Infrastructure;
using HotelBookingSystem.Infrastructure.Identity.Models;
using HotelBookingSystem.Infrastructure.Identity.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace HotelBookingSystem.Api;

/// <summary>
/// The main entry point for the application.
/// </summary>
public class Program
{
    /// <summary>
    /// The application's main method.
    /// </summary>
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Application & Infrastructure Layers
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        // Controllers & Swagger
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Hotel Booking API"
            });

            // Load XML docs for Swagger
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        // JWT Authentication Setup
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
            ?? throw new InvalidOperationException("Missing JwtSettings configuration.");

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings!.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });

        builder.Services.AddAuthorization();

        // Exception Handlers Registration
        builder.Services.AddTransient<IExceptionHandler, ValidationExceptionHandler>();
        builder.Services.AddTransient<IExceptionHandler, IdentityExceptionHandler>();
        builder.Services.AddTransient<IExceptionHandler, DefaultExceptionHandler>();

        var app = builder.Build();

        // Seed Identity Roles & Admin User
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await IdentitySeeder.SeedAsync(userManager, roleManager);
        }

        // Swagger in Development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Global Exception Handling Middleware
        app.UseMiddleware<ExceptionMiddleware>();

        // Standard ASP.NET Core Pipeline
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
