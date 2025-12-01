
using HotelBookingSystem.Api.Middleware;
using HotelBookingSystem.Api.Services;
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
using Microsoft.OpenApi.Models;
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
            options.SwaggerDoc("v1", new OpenApiInfo
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

            // JWT Authentication support in Swagger UI
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            // Require JWT for protected endpoints
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        // JWT Authentication Setup
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
            ?? throw new InvalidOperationException("Missing JwtSettings configuration.");

        // Configure JWT bearer authentication
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

        // Allow reading HttpContext inside services
        builder.Services.AddHttpContextAccessor();

        // Register CurrentUserService (reads user ID from JWT)
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Exception Handlers Registration
        builder.Services.AddTransient<IExceptionHandler, ValidationExceptionHandler>();
        builder.Services.AddTransient<IExceptionHandler, IdentityExceptionHandler>();
        builder.Services.AddTransient<IExceptionHandler, DuplicateRecordExceptionHandler>();
        builder.Services.AddTransient<IExceptionHandler, NotFoundExceptionHandler>();
        builder.Services.AddTransient<IExceptionHandler, UnauthorizedAccessExceptionHandler>();
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

        // HTTPS redirection
        app.UseHttpsRedirection();

        // JWT Authentication + Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
