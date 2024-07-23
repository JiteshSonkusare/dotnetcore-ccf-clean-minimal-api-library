namespace CCFClean.Minimal.Definition.CustomAttributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class ApiEndpointDeprecate(
	string? message = null)
	: Attribute
{
	public string? Message { get; } = message;
}