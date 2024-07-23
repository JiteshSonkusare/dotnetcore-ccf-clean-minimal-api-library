using Microsoft.OpenApi.Models;
using CCFClean.Swagger.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;

namespace CCFClean.Swagger.OpenApi;

public static class ConfigureSwaggerSecurity
{
	public static SwaggerGenOptions AddSwaggerSecurityDefination(this SwaggerGenOptions options, SecuritySchemeParams securitySchemeParams)
	{
		options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
		{
			Name = "Authorization",
			In = securitySchemeParams.ParameterLocation ?? ParameterLocation.Header,
			Type = securitySchemeParams.SecuritySchemeType ?? SecuritySchemeType.Http,
			Scheme = securitySchemeParams.Scheme ?? "Bearer",
			BearerFormat = securitySchemeParams.BearerFormat ?? "JWT",
			Description = $"Input your {securitySchemeParams.Scheme} token in this format - {securitySchemeParams.Scheme} <your-token-here>",
		});
		options.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = securitySchemeParams.ReferenceType ?? ReferenceType.SecurityScheme,
						Id = securitySchemeParams.Scheme ?? "Bearer",
					},
					Scheme = securitySchemeParams.Scheme ?? "Bearer",
					Name = securitySchemeParams.Scheme ?? "Bearer",
					In = securitySchemeParams.ParameterLocation ?? ParameterLocation.Header,
				}, new List<string>()
			},
		});

		return options;
	}
}
