using Microsoft.OpenApi.Models;
using CCFClean.Swagger.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;

namespace CCFClean.Swagger.OpenApi;

public static class ConfigureSwaggerSecurity
{
	public static SwaggerGenOptions AddSwaggerSecurityDefination(this SwaggerGenOptions options, SecuritySchemeParams securitySchemeParams)
	{
		options.AddSecurityDefinition(securitySchemeParams.Scheme, new OpenApiSecurityScheme
		{
			Name         = securitySchemeParams.Name ?? "Authorization",
			Description  = securitySchemeParams.Description ?? $"Input your Bearer token in this format - Bearer <your-token-here>",
			In           = securitySchemeParams.ParameterLocation ?? ParameterLocation.Header,
			Type         = securitySchemeParams.SecuritySchemeType ?? SecuritySchemeType.Http,
			Scheme       = securitySchemeParams.Scheme ?? "Bearer",
			BearerFormat = securitySchemeParams.BearerFormat ?? string.Empty,
		});
		var scheme = new OpenApiSecurityScheme
		{
			Reference = new OpenApiReference
			{
				Type = securitySchemeParams.ReferenceType ?? ReferenceType.SecurityScheme,
				Id   = securitySchemeParams.Scheme ?? "Bearer",
			},
			Scheme = securitySchemeParams.Scheme ?? "Bearer",
			Name   = securitySchemeParams.Scheme ?? "Bearer",
			In     = securitySchemeParams.ParameterLocation ?? ParameterLocation.Header,
		};
		var requirements = new OpenApiSecurityRequirement
		{
			{
				scheme,
				securitySchemeParams.SecuritySchemevalues ?? []
			}
		};
		options.AddSecurityRequirement(requirements);

		return options;
	}
}
