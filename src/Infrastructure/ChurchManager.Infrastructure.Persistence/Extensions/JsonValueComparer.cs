using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

public class JsonValueComparer<T> : ValueComparer<T>
{
    private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public JsonValueComparer() : base(
        (l, r) => CompareJson(l, r),
        v => v == null ? 0 : JsonSerializer.Serialize(v, _options).GetHashCode(),
        v => DeepCopy(v))
    {
    }

    private static bool CompareJson(T left, T right)
    {
        if (left == null && right == null)
            return true;
        if (left == null || right == null)
            return false;
        return JsonSerializer.Serialize(left, _options) == JsonSerializer.Serialize(right, _options);
    }

    private static T DeepCopy(T value)
    {
        if (value == null)
            return default;
        var serialized = JsonSerializer.Serialize(value, _options);
        return JsonSerializer.Deserialize<T>(serialized, _options);
    }
}