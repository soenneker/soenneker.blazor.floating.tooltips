using Intellenum;

namespace Soenneker.Blazor.Floating.Tooltips.Enums;

[Intellenum<string>]
public sealed partial class FloatingTooltipTheme
{
    public static readonly FloatingTooltipTheme Dark = new("dark");
    public static readonly FloatingTooltipTheme Light = new("light");
    public static readonly FloatingTooltipTheme Info = new("info");
    public static readonly FloatingTooltipTheme Success = new("success");
    public static readonly FloatingTooltipTheme Warning = new("warning");
    public static readonly FloatingTooltipTheme Error = new("error");
}