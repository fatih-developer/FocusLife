using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;

namespace FocusLifePlus.API.Filters;

/// <summary>
/// Swagger UI'da model örnek değerlerini göstermek için kullanılan filtre
/// </summary>
public class SwaggerExampleSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null || !schema.Properties.Any())
            return;

        // Her property için örnek değer oluştur
        foreach (var property in schema.Properties)
        {
            if (property.Value.Example == null && property.Value.Default == null)
            {
                // Property tipi için varsayılan örnek değer ata
                property.Value.Example = GetExampleValue(property.Value.Type, property.Key);
            }
        }
    }

    private static IOpenApiAny GetExampleValue(string type, string propertyName)
    {
        return type?.ToLower() switch
        {
            "string" => new OpenApiString(GetStringExample(propertyName)),
            "integer" => new OpenApiInteger(42),
            "number" => new OpenApiDouble(42.42),
            "boolean" => new OpenApiBoolean(true),
            "array" => new OpenApiArray(),
            _ => null
        };
    }

    private static string GetStringExample(string propertyName)
    {
        return propertyName.ToLower() switch
        {
            var name when name.Contains("email") => "user@example.com",
            var name when name.Contains("password") => "P@ssw0rd123",
            var name when name.Contains("username") => "johndoe",
            var name when name.Contains("firstname") => "John",
            var name when name.Contains("lastname") => "Doe",
            var name when name.Contains("phone") => "+90 555 123 4567",
            var name when name.Contains("description") => "Örnek açıklama",
            var name when name.Contains("title") => "Örnek başlık",
            var name when name.Contains("color") => "#FF5733",
            var name when name.Contains("token") => "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
            _ => "Örnek değer"
        };
    }
} 