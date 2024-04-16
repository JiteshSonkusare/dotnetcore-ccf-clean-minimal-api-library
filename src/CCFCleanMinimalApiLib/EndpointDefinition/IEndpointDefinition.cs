using Microsoft.AspNetCore.Builder;

namespace CCFClean.Minimal.Definition;

public interface IEndpointDefinition
{
	void DefineEndpoints(AppBuilderDefinition app); 
	void DefineServices(WebApplicationBuilder builder);
}