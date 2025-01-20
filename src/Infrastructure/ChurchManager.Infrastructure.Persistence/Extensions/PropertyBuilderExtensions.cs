using System.Text.Json;
using ChurchManager.Domain.Common;
using Codeboss.Types;
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
    
    public static PropertyBuilder<string> HasRecordStatus(this PropertyBuilder<string> propertyBuilder)
    {
        return propertyBuilder
            .HasConversion(
                v => v.ToString(),
                v => new RecordStatus(v));
    }
    
    public static PropertyBuilder<TEnum> HasEnumerationConversion<TEnum>(this PropertyBuilder<TEnum> propertyBuilder)
        where TEnum : Enumeration<TEnum, string>, new()
    {
        return propertyBuilder
            .HasConversion(
                v => v.ToString(), // Converts the enumeration to its string representation
                v => new TEnum { Value = v }); // Creates a new enumeration instance from the string value
    }
}