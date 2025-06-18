using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChurchManager.Api.Extensions.JsonConverter;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (string.IsNullOrWhiteSpace(str))  throw new FormatException($"Invalid DateOnly format: '{str}'");

        if (DateTime.TryParse(str, out var dt)) return DateOnly.FromDateTime(dt);

        if (DateOnly.TryParse(str, out var d)) return d;

        throw new FormatException($"Invalid DateOnly format: {str}");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}