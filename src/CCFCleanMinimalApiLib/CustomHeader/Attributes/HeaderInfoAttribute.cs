using Microsoft.OpenApi.Models;

namespace CCFClean.Minimal.CustomHeader;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class HeaderInfoAttribute(string headerName, string dataType) : Attribute
{
    public string Name { get; private set; } = headerName;
    public string DataType { get; private set; } = dataType;
    public bool IsRequired { get; set; } = false;
    public string? Description { get; set; }
    public string? DataFormat { get; set; }
    public ParameterLocation ParameterIn { get; set; } = ParameterLocation.Header;
    public string? AllowedValues { get; set; }
    public string? DefaultValue { get; set; }
}