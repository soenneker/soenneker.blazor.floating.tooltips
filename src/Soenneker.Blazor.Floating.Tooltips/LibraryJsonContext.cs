using Soenneker.Blazor.Floating.Tooltips.Options;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json;
using System;

namespace Soenneker.Blazor.Floating.Tooltips;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, ReadCommentHandling = JsonCommentHandling.Skip, UseStringEnumConverter = true, Converters = new[] { typeof(FloatingTooltipPlacementMetadataConverter), typeof(FloatingTooltipThemeMetadataConverter) })]
[JsonSerializable(typeof(FloatingTooltipOptions))]
[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(double))]
[JsonSerializable(typeof(decimal))]
[JsonSerializable(typeof(float))]
[JsonSerializable(typeof(System.Text.Json.JsonElement))]
[JsonSerializable(typeof(System.Collections.Generic.Dictionary<string, object?>))]
[JsonSerializable(typeof(System.Collections.Generic.List<object?>))]
[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(object[]))]
internal partial class LibraryJsonContext : JsonSerializerContext
{
    internal static JsonTypeInfo<T> Get<T>() =>
        (JsonTypeInfo<T>)(Default.GetTypeInfo(typeof(T)) ?? throw new NotSupportedException($"No generated JSON metadata for {typeof(T)}."));

    internal static JsonSerializerOptions WithContext(JsonSerializerContext? additionalContext)
    {
        JsonSerializerOptions defaults = Get<object>().Options;
        if (additionalContext is null)
            return defaults;
        var options = new JsonSerializerOptions(defaults)
        {
            TypeInfoResolver = JsonTypeInfoResolver.Combine(defaults.TypeInfoResolver!, additionalContext)
        };
        options.MakeReadOnly();
        return options;
    }
}

internal sealed class FloatingTooltipPlacementMetadataConverter : JsonConverter<Soenneker.Blazor.Floating.Tooltips.Enums.FloatingTooltipPlacement>
{
    public override Soenneker.Blazor.Floating.Tooltips.Enums.FloatingTooltipPlacement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.String && Soenneker.Blazor.Floating.Tooltips.Enums.FloatingTooltipPlacement.TryFromValue(reader.GetString(), out var value) ? value : throw new JsonException("Unknown FloatingTooltipPlacement value.");

    public override void Write(Utf8JsonWriter writer, Soenneker.Blazor.Floating.Tooltips.Enums.FloatingTooltipPlacement value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.Value);
}

internal sealed class FloatingTooltipThemeMetadataConverter : JsonConverter<Soenneker.Blazor.Floating.Tooltips.Enums.FloatingTooltipTheme>
{
    public override Soenneker.Blazor.Floating.Tooltips.Enums.FloatingTooltipTheme Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.String && Soenneker.Blazor.Floating.Tooltips.Enums.FloatingTooltipTheme.TryFromValue(reader.GetString(), out var value) ? value : throw new JsonException("Unknown FloatingTooltipTheme value.");

    public override void Write(Utf8JsonWriter writer, Soenneker.Blazor.Floating.Tooltips.Enums.FloatingTooltipTheme value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.Value);
}
