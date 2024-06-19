using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using CCFClean.Minimal.CustomHeader;
using CCFClean.Swagger.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CCFClean.Minimal.Definition.CustomAttributes;

namespace CCFClean.Swagger.OpenApi;

public class SwaggerOperationFilter : IOperationFilter
{
	private readonly OpenApiConfig _openApiConfig;

	public SwaggerOperationFilter(OpenApiConfig openApiConfig)
	{
		_openApiConfig = openApiConfig;
	}

	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		var apiDescription = context.ApiDescription;

		operation.Deprecated = IsDeprecated(apiDescription) | apiDescription.IsDeprecated();

		foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
		{
			var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
			var response = operation.Responses[responseKey];

			foreach (var contentType in response.Content.Keys)
			{
				if (!responseType.ApiResponseFormats.Any(x => x.MediaType == contentType))
				{
					response.Content.Remove(contentType);
				}
			}
		}

		if (operation.Parameters == null)
		{
			return;
		}
		foreach (var parameter in operation.Parameters)
		{
			var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

			parameter.Description ??= description.ModelMetadata?.Description;

			if (parameter.Schema.Default == null &&
				 description.DefaultValue != null &&
				 description.DefaultValue is not DBNull &&
				 description.ModelMetadata is ModelMetadata modelMetadata)
			{
				var json = System.Text.Json.JsonSerializer.Serialize(description.DefaultValue, modelMetadata.ModelType);
				parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
			}

			parameter.Required |= description.IsRequired;
		}

		if (_openApiConfig.EnableGlobalHeader)
			SetGlobalHeaders(operation);
	}

	private static bool IsDeprecated(ApiDescription apiDescription)
	{
		return apiDescription.ActionDescriptor.EndpointMetadata
			.OfType<EndpointDeprecateAttribute>()
			.Any();
	}

	private static void SetGlobalHeaders(OpenApiOperation operation)
	{
		var globalHeaderType = GlobalHeaderExtensions.GetGlobalHeadersType();
		var headerProperties = globalHeaderType.GetGlobalHeaderProperties();

		foreach (var (Property, HeaderInfo) in headerProperties)
		{
			var headerInfo = HeaderInfo;
			if (headerInfo != null)
			{
				var schema = new OpenApiSchema
				{
					Type = headerInfo.DataType,
					Format = headerInfo.DataFormat,
					Default = string.IsNullOrEmpty(headerInfo.DefaultValue) ? null : new OpenApiString(headerInfo.DefaultValue),
					Enum = headerInfo.AllowedValues?.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(V => (IOpenApiAny)new OpenApiString(V)).ToList()
				};

				operation.Parameters ??= new List<OpenApiParameter>();
				operation.Parameters.Add(new OpenApiParameter
				{
					Name = headerInfo.Name,
					In = headerInfo.ParameterIn,
					Required = headerInfo.IsRequired,
					Description = headerInfo.Description,
					Schema = schema
				});
			}
		}
	}
}