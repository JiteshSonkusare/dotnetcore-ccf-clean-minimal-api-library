using Microsoft.AspNetCore.Builder;
using CCFCleanAPITemplate.EndpointDefinition.Models;

namespace CCFClean.Minimal.EndpointDefinition;

public interface IEndpointDefinition
{
	void DefineEndpoints(AppBuilderDefinition app); 
	void DefineServices(WebApplicationBuilder builder);
}