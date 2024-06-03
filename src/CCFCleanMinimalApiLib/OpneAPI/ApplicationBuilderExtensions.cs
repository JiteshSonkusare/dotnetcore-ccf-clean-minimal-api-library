using Microsoft.AspNetCore.Builder;

namespace CCFClean.Swagger;

public static class ApplicationBuilderExtensions
{
	/// <summary>
	/// Use CCF Swagger to configure options.
	/// </summary>
	/// <param name="app"></param>
	/// <param name="swaggerOptions"></param>
	/// <returns></returns>
	public static WebApplication UseCCFSwagger(this WebApplication app, Action<SwaggerConfigOptions>? swaggerOptions = null)
	{
		SwaggerConfigOptions swaggerConfigOptions = new();
		if (swaggerOptions != null)
			swaggerConfigOptions = Minimal.Definition.Extensions.InvokeConfigureOptions(swaggerOptions);

		app.UseSwagger()
			.UseSwaggerUI(options =>
			{
				options.DocumentTitle = swaggerConfigOptions.DocumentTitle;
				if (swaggerConfigOptions.ModelSchemaHide)
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