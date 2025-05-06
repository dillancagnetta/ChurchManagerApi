using System.Globalization;
using System.Text.Json;

namespace ChurchManager.Api.Extensions.JsonConverter;

public class TimeOnlyJsonConverter : System.Text.Json.Serialization.JsonConverter<TimeOnly>
{
    private const string Format = "HH:mm";


    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (string.IsNullOrWhiteSpace(str))  throw new FormatException($"Invalid TimeOnly format: '{str}'");

        if (TimeOnly.TryParseExact(
                str,
                new[] { "HH:mm", "HH:mm:ss" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var time))
        {
            return time;
        }
        
        return TimeOnly.ParseExact(reader.GetString()!, Format);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}