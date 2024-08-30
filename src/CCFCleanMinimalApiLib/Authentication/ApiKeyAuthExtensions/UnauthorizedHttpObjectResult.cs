using Microsoft.AspNetCore.Http;

namespace CCFCleanMinimalApiLib.ApiKeyAuthentication;

public sealed class UnauthorizedHttpObjectResult(
	object body)
	: IResult,
	IStatusCodeHttpResult
{
	private readonly object _body = body;

	public static int StatusCode => StatusCodes.Status401Unauthorized;

	int? IStatusCodeHttpResult.StatusCode => StatusCode;

	public Task ExecuteAsync(HttpContext httpContext)
	{
		ArgumentNullException.ThrowIfNull(httpContext);

		httpContext.Response.StatusCode = StatusCode;
		httpContext.Items[ApiKeyAuthenticationConstants.UnauthorizedMessage] = _body;

		return Task.CompletedTask;
	}
}