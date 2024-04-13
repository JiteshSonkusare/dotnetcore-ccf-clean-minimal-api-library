using Microsoft.AspNetCore.Builder;

namespace CCFClean.Swagger;

public static class ApplicationBuilderExtensions
{
	public static WebApplication UseCCFSwagger(this WebApplication app, string documentTitle = "SwaggerUI", bool modelSchemaHide = false)
	{
		app.UseSwagger()
			.UseSwaggerUI(options =>
			{
				options.DocumentTitle = documentTitle;
				if (modelSchemaHide)
					options.DefaultModelsExpandDepth(-1);
				var descriptions = app.DescribeApiVersions().OrderByDescending(v => v.ApiVersion);
				foreach (var description in descriptions)
				{
					var url = $"{description.GroupName}/swagger.json";
					var name = description.GroupName.ToUpperInvariant();
					options.SwaggerEndpoint(url, name);
				}
			});

		return app;
	}
}