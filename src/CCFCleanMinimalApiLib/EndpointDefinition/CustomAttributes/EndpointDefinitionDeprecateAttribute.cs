namespace CCFClean.Minimal.Definition.CustomAttributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class DefinitionDeprecate(
	string? message = null)
	: Attribute
{
	public string? Message { get; } = message;
}