using System.Text.Json;
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

	internal static JsonSerializerOptions? globalJsonSerializerOptions;

	public static void SetGlobalJsonSerializerSettings(JsonSerializerOptions jsonSerializerOptions, JsonIgnoreCondition jsonIgnoreCondition = JsonIgnoreCondition.Never)
	{
		globalJsonSerializerOptions = jsonSerializerOptions ?? throw new ArgumentNullException(nameof(jsonSerializerOptions), "Null Json serializer options are not allowed.");
		globalJsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
		globalJsonSerializerOptions.DefaultIgnoreCondition = jsonIgnoreCondition;
		_ = globalJsonSerializerOptions.ToJson();
	}
}