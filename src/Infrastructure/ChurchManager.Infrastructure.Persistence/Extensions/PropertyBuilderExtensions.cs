using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<Dictionary<string, object>> HasJsonConversion(
        this PropertyBuilder<Dictionary<string, object>> propertyBuilder,
        JsonSerializerOptions options = null)
    {
        return propertyBuilder.HasConversion(
            v => JsonSerializer.Serialize(v, options),
            v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, options) ?? new Dictionary<string, object>());
    }
}