using CCFClean.Swagger.OpenApi;
using CCFClean.Swagger.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Json;
using CCFClean.Swagger.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;

namespace CCFClean.Swagger;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCCFSwagger(this IServiceCollection services, OpenApiConfig openApiConfig)
	{
		services
			.AddSingleton(openApiConfig)
			.Configure<JsonOptions>(options =>
			{
				Extension.SetGlobalJsonSerializerSettings(options.SerializerOptions);
			})
			.AddEndpointsApiExplorer()
			.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
			.AddSwaggerGen(options =>
			{
				options.EnableAnnotations();
				options.OperationFilter<SwaggerDefaultValues>();
				options.DocumentFilter<SwaggerDocumentFilter>();
			});

		return services;
	}
}