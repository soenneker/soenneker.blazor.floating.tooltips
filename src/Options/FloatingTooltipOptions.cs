using Soenneker.Blazor.Floating.Tooltips.Enums;
using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Floating.Tooltips.Options;

/// <summary>
/// Options for configuring a floating tooltip.
/// </summary>
public class FloatingTooltipOptions
{
    /// <summary>
    /// Tooltip placement relative to the reference element (e.g. "top", "bottom-start", "right-end").
    /// </summary>
    [JsonPropertyName("placement")]
    public FloatingTooltipPlacement Placement { get; set; } = FloatingTooltipPlacement.Top;

    /// <summary>
    /// Whether the tooltip should animate on show/hide.
    /// </summary>
    [JsonPropertyName("animate")]
    public bool Animate { get; set; } = true;

    /// <summary>
    /// Delay in milliseconds before showing the tooltip (if not manually triggered).
    /// </summary>
    [JsonPropertyName("showDelay")]
    public int ShowDelay { get; set; }

    /// <summary>
    /// Delay in milliseconds before hiding the tooltip (if not manually triggered).
    /// </summary>
    [JsonPropertyName("hideDelay")]
    public int HideDelay { get; set; }

    /// <summary>
    /// Whether to render the arrow pointing to the reference element.
    /// </summary>
    [JsonPropertyName("showArrow")]
    public bool ShowArrow { get; set; } = true;

    /// <summary>
    /// Whether the tooltip can receive pointer events (e.g. buttons/links inside).
    /// </summary>
    [JsonPropertyName("interactive")]
    public bool Interactive { get; set; } = false;

    /// <summary>
    /// Whether this tooltip is enabled. If false, nothing is shown.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = true;

    [JsonPropertyName("theme")]
    public FloatingTooltipTheme Theme { get; set; } = FloatingTooltipTheme.Dark;

    /// <summary>
    /// Optional maximum width (in pixels) of the tooltip.
    /// </summary>
    [JsonPropertyName("maxWidth")]
    public int? MaxWidth { get; set; }

    /// <summary>
    /// If true, tooltip will only show or hide when triggered manually via C#.
    /// </summary>
    [JsonPropertyName("manualTrigger")]
    public bool ManualTrigger { get; set; } = false;

    /// <summary>
    /// If true, resources like scripts and styles are loaded from CDN.
    /// </summary>
    [JsonPropertyName("useCdn")]
    public bool UseCdn { get; set; } = true;
}