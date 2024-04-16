using Microsoft.AspNetCore.Builder;

namespace CCFClean.Minimal.EndpointDefinition;

public interface IEndpointDefinition
{
	void DefineEndpoints(AppBuilderDefinition app); 
	void DefineServices(WebApplicationBuilder builder);
}