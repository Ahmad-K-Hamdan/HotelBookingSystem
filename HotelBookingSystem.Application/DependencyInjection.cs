using AutoMapper.Internal;
using FluentValidation;
using HotelBookingSystem.Application.Behaviors;
using HotelBookingSystem.Application.Common.Exceptions.Handlers;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Mappings;
using MediatR;
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

        // Register the behavior into MediatR pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Auto mapper
        // Turned off auto mapping because it threw an error while mapping DTOs
        services.AddAutoMapper(cfg =>
        {
            cfg.Internal().MethodMappingEnabled = false;
        }, typeof(UserMappingProfile).Assembly);

        return services;
    }
}
