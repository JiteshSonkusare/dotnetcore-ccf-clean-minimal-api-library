using Asp.Versioning;

namespace CCFClean.ApiVersioning;

public static class Extensions
{
	public static ApiVersioningOptions AddSunsetPolicy(this ApiVersioningOptions options, SunsetPolicyOptions? sunsetPolicyOptions = null)
	{
		if (sunsetPolicyOptions is not null)
		{
			options.Policies
			.Sunset(sunsetPolicyOptions.Version, sunsetPolicyOptions.Status)
			.Effective(sunsetPolicyOptions.Effecctive.Year, sunsetPolicyOptions.Effecctive.Month, sunsetPolicyOptions.Effecctive.Day)
			.Link(sunsetPolicyOptions.URL)
			.Title(sunsetPolicyOptions.Title)
			.Type("text/html")
			.Language("en");
		}
		return options;
	}
}

public record SunsetPolicyOptions
{
	public string Title { get; set; } = "Versioning Policy";
	public double Version { get; set; }
	public string Status { get; set; } = string.Empty;
	public SunsetPolicyEffective Effecctive { get; set; } = new SunsetPolicyEffective();
	public string URL { get; set; } = string.Empty;

}

public record SunsetPolicyEffective
{
	public int Year { get; set; }
	public int Month { get; set; }
	public int Day { get; set; }
}