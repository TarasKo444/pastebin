using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pastebin.Api.JsonConverters;

public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>
        DateTimeOffset.ParseExact(reader.GetString()!,
            "yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture);

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeOffset dateTimeValue,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(dateTimeValue.ToString(
            "yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture));
}
