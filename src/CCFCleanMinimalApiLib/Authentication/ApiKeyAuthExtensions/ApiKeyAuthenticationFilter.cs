using Microsoft.AspNetCore.Http;

namespace CCFCleanMinimalApiLib.ApiKeyAuthentication;

public class ApiKeyAuthenticationFilter : IEndpointFilter
{
	public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationConfig.ApiKeyHeaderName, out
			var extractedApiKey))
		{
			return new UnauthorizedHttpObjectResult("API Key Missing");
		}

		if (!ApiKeyAuthenticationConfig.ApiKey.Equals(extractedApiKey))
		{
			return new UnauthorizedHttpObjectResult("Invalid API Key");
		}

		return await next(context);
	}
}