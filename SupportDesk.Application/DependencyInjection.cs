using Microsoft.Extensions.DependencyInjection;

namespace SupportDesk.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		var assembly = typeof(DependencyInjection).Assembly;

		// MediatR
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

		// AutoMapper
		services.AddAutoMapper(cfg => cfg.AddMaps(assembly));

		return services;
	}
}