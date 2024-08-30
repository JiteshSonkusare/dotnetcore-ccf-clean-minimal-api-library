using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CCFCleanMinimalApiLib.ApiKeyAuthentication;

public static class ApiKeyAuthenticationExtensions
{
	public static IServiceCollection ConfigureApiKeyAuthentication(
		this IServiceCollection services,
		IConfiguration configuration,
		string sectionName)
	{
		var options = configuration.GetSection(sectionName).Get<ApiKeyAuthenticationOptions>();
		ApiKeyAuthenticationConfig.SetConfig(options ?? new ApiKeyAuthenticationOptions());
		services.Configure<ApiKeyAuthenticationOptions>(configuration.GetSection(sectionName));
		return services;
	}

	public static IServiceCollection ConfigureApiKeyAuthentication(
		this IServiceCollection services,
		Action<ApiKeyAuthenticationBuilder> configureOptions)
	{
		var builder = new ApiKeyAuthenticationBuilder();
		configureOptions(builder);
		var options = builder.Build();
		ApiKeyAuthenticationConfig.SetConfig(options);
		services.Configure<ApiKeyAuthenticationOptions>(opts =>
		{
			opts.ApiKey = options.ApiKey;
			opts.ApiKeyHeaderName = options.ApiKeyHeaderName;
		});
		return services;
	}
}