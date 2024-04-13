using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace CCFCleanAPITemplate.EndpointDefinition.Models;

public class AppBuilderDefinition
{
	public IEndpointRouteBuilder RouteBuilder { get; set; } = null!;
	public WebApplication App { get; set; } = null!;
}