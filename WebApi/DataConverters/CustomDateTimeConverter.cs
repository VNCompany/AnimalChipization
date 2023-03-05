using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi.DataConverters;

public class NullableDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? parsedDateTime = reader.GetString();
        if (parsedDateTime is not null && DateTime.TryParse(parsedDateTime, out DateTime dateTime))
            return dateTime;
        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value is not null)
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-ddTHH:mm:sszzz"));
        else
            writer.WriteNullValue();
    }
}

public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? parsedDateTime = reader.GetString();
        if (parsedDateTime is not null && DateTime.TryParse(parsedDateTime, out DateTime dateTime))
            return dateTime;
        return default;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:sszzz"));
    }
}