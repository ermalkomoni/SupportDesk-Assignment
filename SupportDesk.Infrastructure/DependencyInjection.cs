using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportDesk.Application.Services;
using SupportDesk.Core.Repositories;
using SupportDesk.Infrastructure.Data;
using SupportDesk.Infrastructure.Repositories;
using SupportDesk.Infrastructure.Services;

namespace SupportDesk.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseNpgsql(
				configuration.GetConnectionString("DefaultConnection"),
				npgsql => npgsql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

		services.AddScoped<IAgentRepository, AgentRepository>();
		services.AddScoped<ITicketRepository, TicketRepository>();
		services.AddScoped<IReferenceNumberGenerator, ReferenceNumberGenerator>();

		return services;
	}
}