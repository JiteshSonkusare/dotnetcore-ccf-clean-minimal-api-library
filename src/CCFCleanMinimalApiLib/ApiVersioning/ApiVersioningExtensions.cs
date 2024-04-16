using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace CCFClean.ApiVersioning;

public record DefineApiVersion(int MajorVersion, int MinorVersion, bool IsVersionDeprecated = false);

internal static class ApiVersioningExtensions
{
	internal static RouteGroupBuilder EndpointRouteBuilder(this WebApplication app, IEnumerable<DefineApiVersion>? apiVersions, string? apiPathPrefix)
	{
		ApiVersionSet apiVersionSet = app.NewApiVersionSet()
										 .WithApiVersions(apiVersions)
										 .ReportApiVersions()
										 .Build();

		RouteGroupBuilder versionedGroup = app.MapGroup($"{apiPathPrefix}/v{{version:apiVersion}}")
											  .WithApiVersionSet(apiVersionSet);

		return versionedGroup;
	}

	private static ApiVersionSetBuilder WithApiVersions(this ApiVersionSetBuilder builder, IEnumerable<DefineApiVersion>? apiVersions)
	{
		if (apiVersions is not null)
		{
			foreach (var version in apiVersions)
			{
				builder.HasApiVersion(version.MajorVersion, version.MinorVersion);
				if (version.IsVersionDeprecated)
					builder.HasDeprecatedApiVersion(version.MajorVersion);
			}
		}
		return builder;
	}
}