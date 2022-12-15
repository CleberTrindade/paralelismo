using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Paralelismo.Contas.Application.Services;
using Paralelismo.Contas.Domain.Interfaces;
using Paralelismo.Contas.Infra.Data.Repository;

namespace Paralelismo.Contas.Infra.IoC;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<IContaClienteRepository, ContaClienteRepository>();

		return services;
	}

	public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<IContaClienteService, ContaClienteService>();

		return services;
	}
}
