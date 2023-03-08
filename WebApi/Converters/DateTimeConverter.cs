/*
 Конвертеры для DateTime и Nullable<DateTime> 
 Предназначены для корректной сериализации и десериализации JSON параметров
 даты и времени в формате ISO-8601 опуская миллисекунды
 */


using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi.Converters;

internal static class __DateTimeConverterMethods
{
    public static DateTime? Read(string? dateTimeString)
    {
        if (dateTimeString is not null && DateTime.TryParse(dateTimeString, out DateTime dateTime))
            return new DateTime(
                dateTime.Year, dateTime.Month, dateTime.Day,
                dateTime.Hour, dateTime.Minute, dateTime.Second,
                DateTimeKind.Local);
        return null;
    }

    public static string? Write(DateTime? dateTime)
        => dateTime?.ToString("yyyy-MM-ddTHH:mm:sszzz");
}

public class NullableDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => __DateTimeConverterMethods.Read(reader.GetString());
    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        string? formattedDateTime = __DateTimeConverterMethods.Write(value);
        if (formattedDateTime is not null) writer.WriteStringValue(formattedDateTime); else writer.WriteNullValue();
    }
}
public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => __DateTimeConverterMethods.Read(reader.GetString()) ?? default;
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(__DateTimeConverterMethods.Write(value) ?? throw new InvalidOperationException());
}