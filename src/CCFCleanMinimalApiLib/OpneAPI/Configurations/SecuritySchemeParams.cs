using Microsoft.OpenApi.Models;

namespace CCFClean.Swagger.Configurations;

public record SecuritySchemeParams
{
    public string? Name { get; set; }
    public ParameterLocation? ParameterLocation { get; set; }
    public SecuritySchemeType? SecuritySchemeType { get; set; }
    public string? Scheme { get; set; }
    public string? BearerFormat { get; set; }
    public ReferenceType? ReferenceType { get; set; }
	public string? Description { get; set; }
	public List<string>? SecuritySchemevalues { get; set; }
}