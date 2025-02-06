using Microsoft.OpenApi.Models;
using CCFClean.Swagger.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CCFClean.Swagger.OpenApi;

public class SwaggerDocumentFilter(
	OpenApiConfig openApiConfig) 
	: IDocumentFilter
{
	private readonly OpenApiConfig _openApiConfig = openApiConfig;

	public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
	{
		if (_openApiConfig.ServerPathFilters != null)
		{
			var useBasePathListFilter = _openApiConfig.ServerPathFilters?.UseBasePathListFilter;

			if (useBasePathListFilter == null || Convert.ToBoolean(useBasePathListFilter))
				swaggerDoc.Servers = SetBasePathListFilterURLs;
			else
				swaggerDoc.Servers = SetCustomePathFilterURLs(
					_openApiConfig?.ServerPathFilters?.CustomeBasePathFilter?.URL ?? string.Empty,
					_openApiConfig?.ServerPathFilters?.CustomeBasePathFilter?.EnvironmentNames ?? []);
		}
		if (_openApiConfig?.SecurityExt != null)
		{
			if (_openApiConfig?.SecurityExt?.NonSecuredVersions != null)
			{
				foreach (var _ in from nonSecuredVersion in _openApiConfig?.SecurityExt?.NonSecuredVersions
								  where context.DocumentName == nonSecuredVersion
								  select new { })
				{
					swaggerDoc.Components.SecuritySchemes.Remove(_openApiConfig?.SecuritySchemeParams?.Scheme ?? "Bearer");
				}
			}
		}
	}

	private IList<OpenApiServer>? SetBasePathListFilterURLs
	{
		get
		{
			var devBasePathFilters = _openApiConfig.ServerPathFilters?.BasePathListFilter;
			if (devBasePathFilters == null)
				return [];

			var openApiServers = devBasePathFilters?.Select(basePath =>
				new OpenApiServer
				{
					Description = basePath.Environment,
					Url = basePath.Url
				})
				.ToList();

			return openApiServers;
		}
	}

	public static IList<OpenApiServer> SetCustomePathFilterURLs(string url, IList<string> envNames)
	{
		var serverVariables = new Dictionary<string, OpenApiServerVariable>
		{
			["Environment"] = new OpenApiServerVariable
			{
				Default = envNames.FirstOrDefault(),
				Description = "Environment identifier.",
				Enum = envNames.ToList()
			}
		};
		var server = new OpenApiServer
		{
			Url = url,
			Variables = serverVariables
		};
		return [server];
	}
}