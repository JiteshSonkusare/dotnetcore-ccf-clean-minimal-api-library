using System.Text.Json;
using Microsoft.OpenApi.Models;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace CCFClean.Swagger.Extensions;

public static class Extension
{
	private static readonly JsonSerializerOptions serializerOptions;

	static Extension()
	{
		serializerOptions = new JsonSerializerOptions();
	}

	public static string ToJson(this object obj)
	{
		return JsonSerializer.Serialize(obj, serializerOptions);
	}

	public static T? ConvertFromJson<T>(this string jsonContent)
	{
		if (string.IsNullOrWhiteSpace(jsonContent))
			return default;
		return JsonSerializer.Deserialize<T>(jsonContent, serializerOptions);
	}

	public static T? ConvertFromJson<T>(this string jsonContent, JsonSerializerOptions? customOptions = null)
	{
		if (string.IsNullOrWhiteSpace(jsonContent))
			return default;
		return JsonSerializer.Deserialize<T>(jsonContent, customOptions ?? globalJsonSerializerOptions);
	}

	public static object? ConvertFromJson(this string jsonContent, Type objectType, JsonSerializerOptions? customOptions = null)
	{
		if (string.IsNullOrWhiteSpace(jsonContent))
			return default;
		return JsonSerializer.Deserialize(jsonContent, objectType, customOptions ?? globalJsonSerializerOptions);
	}

	public static JsonSerializerOptions? globalJsonSerializerOptions;

	public static void SetGlobalJsonSerializerSettings(JsonSerializerOptions jsonSerializerOptions, JsonIgnoreCondition jsonIgnoreCondition = JsonIgnoreCondition.Never)
	{
		globalJsonSerializerOptions = jsonSerializerOptions ?? throw new ArgumentNullException(nameof(jsonSerializerOptions), "Null Json serializer options are not allowed.");
		globalJsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
		globalJsonSerializerOptions.DefaultIgnoreCondition = jsonIgnoreCondition;
		_ = globalJsonSerializerOptions.ToJson();
	}

	/// <summary>
	/// Convert xml to object
	/// </summary>
	/// <param name="xmlContent"></param>
	/// <param name="objectType"></param>
	/// <param name="customOptions"></param>
	/// <returns>Deserilized object data</returns>
	public static T? ConvertFromXml<T>(this string xmlContent)
	{
		if (string.IsNullOrWhiteSpace(xmlContent))
			return default;
		XmlSerializer serializer = new(typeof(T));
		using StringReader reader = new(xmlContent);
		return (T)serializer.Deserialize(reader)!;
	}

	internal static string? GetValueFromContext(HttpContext context, ParameterLocation location, string name)
	{
		return location switch
		{
			ParameterLocation.Header => context.Request.Headers[name].FirstOrDefault(),
			ParameterLocation.Query => context.Request.Query[name].FirstOrDefault(),
			_ => string.Empty,
		};
	}
}