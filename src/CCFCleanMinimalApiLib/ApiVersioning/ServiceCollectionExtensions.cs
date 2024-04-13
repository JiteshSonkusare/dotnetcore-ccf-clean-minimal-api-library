using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace CCFClean.ApiVersioning;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCCFApiVersioning(this IServiceCollection services, ApiVersioningReaderEnum apiVersionReaderEnum = ApiVersioningReaderEnum.UrlSegment, SunsetPolicyOptions? sunsetPolicyOptions = null)
	{
		services.AddApiVersioning(options =>
		{
			if (sunsetPolicyOptions is not null)
				options.AddSunsetPolicy(sunsetPolicyOptions);
			options.DefaultApiVersion = new ApiVersion(1, 0);
			options.AssumeDefaultVersionWhenUnspecified = true;
			options.ReportApiVersions = true;
			options.ApiVersionReader = apiVersionReaderEnum switch
			{
				ApiVersioningReaderEnum.UrlSegment => new UrlSegmentApiVersionReader(),
				ApiVersioningReaderEnum.QueryString => new QueryStringApiVersionReader("version"),
				ApiVersioningReaderEnum.Header => new HeaderApiVersionReader("x-api-version"),
				_ => new UrlSegmentApiVersionReader(),
			};
		}).AddApiExplorer(options =>
		{
			options.GroupNameFormat = "'v'VVV";
			if (apiVersionReaderEnum == ApiVersioningReaderEnum.UrlSegment)
				options.SubstituteApiVersionInUrl = true;
		});

		return services;
	}
}