using Newtonsoft.Json;
using System.Reflection;

namespace Tectum.PublicPaymentProcessClient.Converters;

public class JsonPropertyEnumConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        var enumType = value.GetType();
        var field = enumType.GetField(value.ToString());
        if (field != null)
        {
            var attribute = field.GetCustomAttribute<JsonPropertyAttribute>();
            if (attribute != null)
            {
                writer.WriteValue(attribute.PropertyName);
                return;
            }
        }

        // Fallback to standard enum name
        writer.WriteValue(value.ToString());
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        var value = reader.Value?.ToString();
        if (string.IsNullOrEmpty(value))
            return null;

        var enumType = IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;

        foreach (var field in enumType.GetFields())
        {
            var attribute = field.GetCustomAttribute<JsonPropertyAttribute>();
            if (attribute != null && attribute.PropertyName == value)
            {
                return field.GetValue(null);
            }
        }

        // Fallback to standard enum parsing if no JsonProperty found
        if (Enum.TryParse(enumType, value, true, out var result))
        {
            return result;
        }

        throw new JsonSerializationException($"Value '{value}' is not valid for {enumType.Name}.");
    }

    public override bool CanConvert(Type objectType)
    {
        var type = IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
        return type is { IsEnum: true };
    }

    private bool IsNullableType(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
}
