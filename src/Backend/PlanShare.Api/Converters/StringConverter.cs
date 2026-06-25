using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace PlanShare.Api.Converters;

public partial class StringConverter : JsonConverter<String>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();

        if (value is null)
        {
            return null;
        }

        string result = RemoveExtraWhiteSpaces().Replace(input: value, replacement: " ").Trim();

        return result;
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value: value);
    }

    [GeneratedRegex(pattern: @"\s+")]
    private static partial Regex RemoveExtraWhiteSpaces();
}