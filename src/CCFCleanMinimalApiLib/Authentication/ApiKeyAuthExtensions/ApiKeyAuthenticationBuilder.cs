namespace CCFCleanMinimalApiLib.ApiKeyAuthentication;

public class ApiKeyAuthenticationBuilder
{
	private readonly ApiKeyAuthenticationOptions _options = new();

	public ApiKeyAuthenticationBuilder WithApiKey(string apiKey)
	{
		_options.ApiKey = apiKey;
		return this;
	}

	public ApiKeyAuthenticationBuilder WithHeaderName(string headerName)
	{
		_options.ApiKeyHeaderName = headerName;
		return this;
	}

	public ApiKeyAuthenticationOptions Build() => _options;
}