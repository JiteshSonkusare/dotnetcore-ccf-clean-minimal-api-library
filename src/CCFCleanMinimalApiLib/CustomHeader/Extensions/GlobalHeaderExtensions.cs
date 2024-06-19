using System.Reflection;

namespace CCFClean.Minimal.CustomHeader;

public static class GlobalHeaderExtensions
{
	public static Type? GetGlobalHeadersType()
	{
		return AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(assembly => assembly.GetTypes())
			.FirstOrDefault(type => typeof(IGlobalHeaders).IsAssignableFrom(type)
									&& !type.IsInterface && !type.IsAbstract);
	}

	public static IEnumerable<(PropertyInfo Property, HeaderInfoAttribute HeaderInfo)> GetGlobalHeaderProperties(this Type? globalHeaderType)
	{
		if (globalHeaderType == null) throw new ArgumentNullException(nameof(globalHeaderType));

		return globalHeaderType.GetProperties()
			.Where(p => p.GetCustomAttribute<HeaderInfoAttribute>() != null)
			.Select(p => (Property: p, HeaderInfo: p.GetCustomAttribute<HeaderInfoAttribute>()!));
	}
}