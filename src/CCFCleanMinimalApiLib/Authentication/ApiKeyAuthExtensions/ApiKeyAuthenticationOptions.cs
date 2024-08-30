namespace CCFCleanMinimalApiLib.ApiKeyAuthentication;

public class ApiKeyAuthenticationOptions
{
	public string ApiKey { get; set; } = null!;
	public string ApiKeyHeaderName { get; set; } = "X-Api-Key";
}

public static class ApiKeyAuthenticationConfig
{
	public static string ApiKey { get; private set; } = string.Empty;
	public static string ApiKeyHeaderName { get; private set; } = "X-Api-Key";

	public static void SetConfig(ApiKeyAuthenticationOptions options)
	{
		ApiKey = options.ApiKey;
		ApiKeyHeaderName = options.ApiKeyHeaderName;
	}
}

public static class ApiKeyAuthenticationConstants
{
	public static string DefaultSectionName { get; set; } = "ApiKeyAuthentication";
	public static string UnauthorizedMessage = "UnauthorizedMessage";
}