using Soenneker.Gen.EnumValues;

namespace Soenneker.Blazor.Floating.Tooltips.Enums;

/// <summary>
/// Represents the floating tooltip theme.
/// </summary>
[EnumValue<string>]
public sealed partial class FloatingTooltipTheme
{
    /// <summary>
    /// The dark.
    /// </summary>
    public static readonly FloatingTooltipTheme Dark = new("dark");
    /// <summary>
    /// The light.
    /// </summary>
    public static readonly FloatingTooltipTheme Light = new("light");
    /// <summary>
    /// The info.
    /// </summary>
    public static readonly FloatingTooltipTheme Info = new("info");
    /// <summary>
    /// The success.
    /// </summary>
    public static readonly FloatingTooltipTheme Success = new("success");
    /// <summary>
    /// The warning.
    /// </summary>
    public static readonly FloatingTooltipTheme Warning = new("warning");
    /// <summary>
    /// The error.
    /// </summary>
    public static readonly FloatingTooltipTheme Error = new("error");
}
