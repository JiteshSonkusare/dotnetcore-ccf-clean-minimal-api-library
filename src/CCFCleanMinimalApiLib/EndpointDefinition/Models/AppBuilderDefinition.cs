using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace CCFClean.Minimal.EndpointDefinition;

public class AppBuilderDefinition
{
	public IEndpointRouteBuilder RouteBuilder { get; set; } = null!;
	public WebApplication App { get; set; } = null!;
}