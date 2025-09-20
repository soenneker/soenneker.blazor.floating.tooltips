using Intellenum;

namespace Soenneker.Blazor.Floating.Tooltips.Enums;

/// <summary>
/// Tooltip placement relative to the reference element.
/// </summary>
[Intellenum<string>]
public sealed partial class FloatingTooltipPlacement
{
    public static readonly FloatingTooltipPlacement Top = new("top");
    public static readonly FloatingTooltipPlacement Bottom = new("bottom");
    public static readonly FloatingTooltipPlacement Left = new("left");
    public static readonly FloatingTooltipPlacement Right = new("right");
}
