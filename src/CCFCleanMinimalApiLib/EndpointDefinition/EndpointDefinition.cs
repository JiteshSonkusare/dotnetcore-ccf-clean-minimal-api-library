using System.Reflection;
using CCFClean.ApiVersioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CCFClean.Minimal.Definition.CustomAttributes;

namespace CCFClean.Minimal.Definition;

public static class EndpointDefinition
{
	/// <summary>
	/// Register the CCFClean Minimal Endpoint Definition API pattern.
	/// </summary>
	/// <param name="builder"></param>
	/// <param name="assembly"></param>
	/// <returns></returns>
	public static WebApplicationBuilder AddEndpointDefinitions(this WebApplicationBuilder builder, Assembly assembly)
	{
		var endpointDefinitions = new List<IEndpointDefinition>();

		endpointDefinitions.AddRange(
				assembly.ExportedTypes
				.Where(x => typeof(IEndpointDefinition).IsAssignableFrom(x)
				   && !x.IsInterface
				   && !x.IsAbstract
				   && !x.IsDefined(typeof(EndpointDefinitionDeprecateAttribute), false))
				.Select(Activator.CreateInstance)
				.Cast<IEndpointDefinition>());


		foreach (var endpointDefinition in endpointDefinitions)
		{
			endpointDefinition.DefineServices(builder);
		}

		builder.Services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndpointDefinition>);

		return builder;
	}

	/// <summary>
	/// Use to map the CCFClean Minimal Endpoint Definition API pattern, specifiying required API versions and option to customize the endpoint route path.
	/// </summary>
	/// <param name="app"></param>
	/// <param name="endpointRouteOptions"></param>
	/// <returns></returns>
	public static IApplicationBuilder MapEndpointDefinitions(this WebApplication app, Action<EndpointRouteOptions> endpointRouteOptions)
	{
		var options = Extensions.InvokeConfigureOptions(endpointRouteOptions);

		var routeGroupBuilder = app.EndpointRouteBuilder(options?.ApiVersions, options?.ApiPathPrefix);

		var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>();

		foreach (var endpointDefinition in definitions)
		{
			endpointDefinition.DefineEndpoints(new AppBuilderDefinition() { RouteBuilder = routeGroupBuilder, App = app });
		}

		return app;
	}
}
