using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SupportDesk.Application.Behaviors;

namespace SupportDesk.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		var assembly = ApplicationAssemblyReference.Assembly;

		// MediatR
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

		// AutoMapper
		services.AddAutoMapper(cfg => cfg.AddMaps(assembly));

		// FluentValidation
		services.AddValidatorsFromAssembly(assembly);

		// Registered validators as part of the MediatR pipeline
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

		return services;
	}
}
