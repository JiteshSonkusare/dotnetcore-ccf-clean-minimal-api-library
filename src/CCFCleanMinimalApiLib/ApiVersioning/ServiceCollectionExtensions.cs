﻿using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace CCFClean.ApiVersioning;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Add API versioning (Optionally configure the path segment for API versioning (default is 'UrlSegment') and add sunset policy for the API version.)
	/// </summary>
	/// <param name="services"></param>
	/// <param name="apiVersioning"></param>
	/// <param name="sunsetPolicy"></param>
	/// <returns></returns>
	public static IServiceCollection AddCCFApiVersioning(this IServiceCollection services, Action<ApiVersioningConfigOptions>? apiVersioning = null)
	{
		var apiVersioningConfigOptions = Minimal.Definition.Extensions.InvokeConfigureOptions(apiVersioning);

		services.AddApiVersioning(options =>
		{
			if (apiVersioningConfigOptions is not null)
				options.AddSunsetPolicy(apiVersioningConfigOptions.SunsetPolicyOptions);
			options.DefaultApiVersion = new ApiVersion(1, 0);
			options.AssumeDefaultVersionWhenUnspecified = true;
			options.ReportApiVersions = true;
			options.ApiVersionReader = apiVersioningConfigOptions?.ApiVersionReaderEnum switch
			{
				ApiVersioningReaderEnum.UrlSegment => new UrlSegmentApiVersionReader(),
				ApiVersioningReaderEnum.QueryString => new QueryStringApiVersionReader("version"),
				ApiVersioningReaderEnum.Header => new HeaderApiVersionReader("x-api-version"),
				_ => new UrlSegmentApiVersionReader(),
			};
		}).AddApiExplorer(options =>
		{
			options.GroupNameFormat = "'v'VVV";
			if (apiVersioningConfigOptions.ApiVersionReaderEnum == ApiVersioningReaderEnum.UrlSegment)
				options.SubstituteApiVersionInUrl = true;
		});

		return services;
	}
}