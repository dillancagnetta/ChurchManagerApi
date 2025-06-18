using System.Globalization;
using System.Text.Json;

namespace ChurchManager.Api.Extensions.JsonConverter;

public class UtcIsoDateTimeConverter : System.Text.Json.Serialization.JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-ddTHH:mm:ss.fffffffZ";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Parse incoming string as UTC
        return DateTime.Parse(reader.GetString()!, null, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // Always write in UTC with Z suffix
        writer.WriteStringValue(value.ToUniversalTime().ToString(Format));
    }
}
