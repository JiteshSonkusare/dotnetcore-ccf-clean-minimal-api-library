using CCFClean.Swagger.OpenApi;
using CCFClean.Swagger.Extensions;
using CCFClean.Minimal.Definition;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Json;
using CCFClean.Swagger.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;

namespace CCFClean.Swagger;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Register CCF Swagger and configure options for OpenAPI documentation and Swagger authentication.
	/// </summary>
	/// <param name="services"></param>
	/// <param name="openApiConfig"></param>
	/// <returns></returns>
	public static IServiceCollection AddCCFSwagger(this IServiceCollection services, Action<OpenApiConfig> openApiConfig)
	{
		services
			.AddCCFCleanConfigSingleton(openApiConfig)
			.Configure<JsonOptions>(options =>
			{
				Extension.SetGlobalJsonSerializerSettings(options.SerializerOptions);
			})
			.AddEndpointsApiExplorer()
			.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
			.AddSwaggerGen(options =>
			{
				options.EnableAnnotations();
				options.OperationFilter<SwaggerOperationFilter>();
				options.DocumentFilter<SwaggerDocumentFilter>();
			});

		return services;
	}
}