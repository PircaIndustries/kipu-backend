using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kipu.API.Shared.Infrastructure.Interfaces.ASP.Configuration;

/// <summary>
/// Swagger Operation Filter to add Accept-Language header parameter to all endpoints.
/// </summary>
public class AcceptLanguageHeaderParameterOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<IOpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Accept-Language",
            In = ParameterLocation.Header,
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.String,
                Default = JsonValue.Create("en"),
                Enum = new List<JsonNode>
                {
                    JsonValue.Create("en")!,
                    JsonValue.Create("es")!
                }
            },
            Description = "Preferred language for response translation (e.g. en, es)"
        });
    }
}
