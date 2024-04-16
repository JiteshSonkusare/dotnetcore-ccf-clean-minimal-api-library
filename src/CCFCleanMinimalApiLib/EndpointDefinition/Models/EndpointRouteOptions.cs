using CCFClean.ApiVersioning;

namespace CCFClean.Minimal.Definition;

public record EndpointRouteOptions()
{
    /// <summary>
    /// Define the number of versions supported by this API.
    /// </summary>
    public IEnumerable<DefineApiVersion>? ApiVersions { get; set; }

    /// <summary>
    /// Specify a path prefix to be added to all API endpoints.
    /// </summary>
    public string? ApiPathPrefix { get; set; }
}