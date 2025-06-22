using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Ediki.Application.Behaviors;
using AutoMapper;

namespace Ediki.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(DependencyInjection).Assembly);
        });
        
        services.AddSingleton(mapperConfig);
        services.AddSingleton<IMapper>(provider => new Mapper(mapperConfig));
        
        return services;
    }
}
