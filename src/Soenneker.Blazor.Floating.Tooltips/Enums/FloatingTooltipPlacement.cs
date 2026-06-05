using Soenneker.Gen.EnumValues;

namespace Soenneker.Blazor.Floating.Tooltips.Enums;

/// <summary>
/// Tooltip placement relative to the reference element.
/// </summary>
[EnumValue<string>]
public sealed partial class FloatingTooltipPlacement
{
    /// <summary>
    /// The top.
    /// </summary>
    public static readonly FloatingTooltipPlacement Top = new("top");
    /// <summary>
    /// The bottom.
    /// </summary>
    public static readonly FloatingTooltipPlacement Bottom = new("bottom");
    /// <summary>
    /// The left.
    /// </summary>
    public static readonly FloatingTooltipPlacement Left = new("left");
    /// <summary>
    /// The right.
    /// </summary>
    public static readonly FloatingTooltipPlacement Right = new("right");
}
