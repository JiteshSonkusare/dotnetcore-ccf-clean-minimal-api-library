﻿namespace CCFClean.Swagger.Configurations;

public record SecurityExt
{
	/// <summary>
	/// Set it to true to add Autorize in swagger.
	/// </summary>
	public bool IsSecured { get; set; } = false;

	/// <summary>
	/// Sends API version document name for which authorization is not required. eg: v1 
	/// </summary>
	public IList<string>? NonSecuredVersions { get; set; }
}