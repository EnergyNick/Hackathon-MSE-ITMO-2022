using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace StudentManager.Service.Swagger;

public class SwaggerSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null) return;

        foreach (var (_, value) in schema.Properties)
        {
            if (value.Default != null && value.Example == null)
                value.Example = value.Default;
        }
    }
}