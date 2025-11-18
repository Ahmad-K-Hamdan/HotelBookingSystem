using AutoMapper.Internal;
using FluentValidation;
using HotelBookingSystem.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HotelBookingSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Fluent Validation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Auto mapper
        // Turned off auto mapping because it threw an error while mapping DTOs
        services.AddAutoMapper(cfg =>
        {
            cfg.Internal().MethodMappingEnabled = false;
        }, typeof(UserMappingProfile).Assembly);

        return services;
    }
}
