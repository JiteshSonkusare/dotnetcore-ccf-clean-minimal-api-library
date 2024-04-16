namespace CCFClean.ApiVersioning;

public class ApiVersioningConfigOptions
{
	/// <summary>
	/// Specifies the path segment for API versioning that will be read by the API version reader.
	/// </summary>
	public ApiVersioningReaderEnum ApiVersionReaderEnum { get; set; } = ApiVersioningReaderEnum.UrlSegment;

	/// <summary>
	/// Specify the sunset policy options.
	/// </summary>
	public SunsetPolicyOptions? SunsetPolicyOptions {  get; set; }	
}