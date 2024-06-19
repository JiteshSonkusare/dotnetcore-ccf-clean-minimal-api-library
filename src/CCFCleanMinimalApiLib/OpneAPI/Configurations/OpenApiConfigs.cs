namespace CCFClean.Swagger.Configurations;

/// <summary>
/// Send require swagger configuration values. SecurityVersionExt, OpenApiInfoExt, ServerPathFilters
/// </summary>
public record OpenApiConfig
{
	/// <summary>
	/// Sends options to configure authentication to swagger document.
	/// </summary>
	public SecurityExt? SecurityExt { get; set; }
	/// <summary>
	/// Sends OpenAPI details to show on swagger document.
	/// </summary>
	public OpenApiInfoExt? OpenApiInfoExt { get; set; }
	/// <summary>
	/// Sends server details to show on swagger document.
	/// </summary>
	public ServerPathFilters? ServerPathFilters { get; set; }
	/// <summary>
	/// typeof(class): Set global headers to api all endpoints.
	/// </summary>
	public bool EnableGlobalHeader { get; set; } = false;
}