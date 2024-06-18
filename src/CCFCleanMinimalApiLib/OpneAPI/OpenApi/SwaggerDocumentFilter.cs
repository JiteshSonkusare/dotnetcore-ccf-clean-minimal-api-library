using Microsoft.OpenApi.Models;
using CCFClean.Swagger.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CCFClean.Swagger.OpenApi;

public class SwaggerDocumentFilter : IDocumentFilter
{
	private readonly OpenApiConfig _openApiConfig;

	public SwaggerDocumentFilter(OpenApiConfig openApiConfig)
	{
		_openApiConfig = openApiConfig;
	}

	public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
	{
		if (_openApiConfig.ServerPathFilters != null)
		{
			var isBasePathListFilter = _openApiConfig.ServerPathFilters?.IsBasePathListFilter;

			if (isBasePathListFilter == null || Convert.ToBoolean(isBasePathListFilter))
				swaggerDoc.Servers = GetBasePathListFilterURLs();
			else
				swaggerDoc.Servers = GetCustomeBasePathFilterURLs(_openApiConfig?.ServerPathFilters?.CustomeBasePathFilter?.URL ?? string.Empty, _openApiConfig?.ServerPathFilters?.CustomeBasePathFilter?.EnvironmentNames ?? new List<string>());
		}
		if (_openApiConfig?.SecurityExt != null)
		{
			if (_openApiConfig?.SecurityExt?.NonSecuredVersions != null)
			{
				foreach (var _ in from nonSecuredVersion in _openApiConfig?.SecurityExt?.NonSecuredVersions
								  where context.DocumentName == nonSecuredVersion
								  select new { })
				{ 
					swaggerDoc.Components.SecuritySchemes.Remove("Bearer");
				}
			}
		}
	}

	private IList<OpenApiServer>? GetBasePathListFilterURLs()
	{
		var devBasePathFilters = _openApiConfig.ServerPathFilters?.BasePathListFilter;
		if (devBasePathFilters == null)
			return new List<OpenApiServer>();

		var openApiServers = devBasePathFilters?.Select(basePath =>
			new OpenApiServer
			{
				Description = basePath.Environment,
				Url = basePath.Url
			})
			.ToList();

		return openApiServers;
	}

	public static IList<OpenApiServer> GetCustomeBasePathFilterURLs(string url, IList<string> envNames)
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
		return new List<OpenApiServer> { server };
	}
}