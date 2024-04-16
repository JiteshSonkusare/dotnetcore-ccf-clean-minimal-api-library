using Microsoft.Extensions.DependencyInjection;

namespace CCFClean.Minimal.Definition;

public static class Extensions
{
	public static IServiceCollection AddCCFCleanConfigSingleton<T>(this IServiceCollection services, Action<T> configureObject) where T : class, new()
	{
		var obj = new T();
		configureObject(obj);
		services.AddSingleton<T>(provider => obj);

		return services;
	}

	public static TOptions InvokeConfigureOptions<TOptions>(Action<TOptions> configureAction) where TOptions : new()
	{
		var options = new TOptions();
		configureAction?.Invoke(options);
		return options;
	}
}