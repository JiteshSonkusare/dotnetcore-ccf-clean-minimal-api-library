using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;

namespace CCFClean.Minimal.CustomHeader;

public class CustomHeaderMiddleware(RequestDelegate next)
{
	private readonly RequestDelegate _next = next;

	public async Task InvokeAsync(HttpContext context, IGlobalHeaders globalHeaders)
	{
		var globalHeaderType = GlobalHeaderExtensions.GetGlobalHeadersType();

		if (globalHeaderType == null)
		{
			await _next(context);
			return;
		}

		var headerProperties = globalHeaderType.GetGlobalHeaderProperties();
		foreach (var (Property, HeaderInfo) in headerProperties)
		{
			if (HeaderInfo != null)
			{
				var value = GetValueFromContext(context, HeaderInfo.ParameterIn, HeaderInfo.Name);
				if (!string.IsNullOrEmpty(value))
				{
					globalHeaders.AddCustomHeader(Property.Name, value);
				}
			}
		}

		context.Items[globalHeaderType.Name] = globalHeaders;

		await _next(context);
	}

	private static string? GetValueFromContext(HttpContext context, ParameterLocation location, string name)
	{
		return location switch
		{
			ParameterLocation.Header => context.Request.Headers[name].FirstOrDefault(),
			ParameterLocation.Query => context.Request.Query[name].FirstOrDefault(),
			_ => string.Empty,
		};
	}
}
