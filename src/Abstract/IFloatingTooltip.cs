using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Soenneker.Blazor.Floating.Tooltips.Enums;
using Soenneker.Blazor.Floating.Tooltips.Options;

namespace Soenneker.Blazor.Floating.Tooltips.Abstract;

/// <summary>
/// Represents a floating tooltip component instance with customizable appearance, behavior, and lifecycle methods.
/// </summary>
public interface IFloatingTooltip : IAsyncDisposable
{
    /// <summary>
    /// The tooltip content as a plain string. Mutually exclusive with <see cref="SetTooltipContent"/>.
    /// </summary>
    string? Text { get; set; }

    /// <summary>
    /// The unique identifier used internally for tooltip registration and DOM references.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// Optional parameters applied to the tooltip container.
    /// </summary>
    Dictionary<string, object?>? TooltipAttributes { get; set; }

    /// <summary>
    /// Optional parameters applied to the tooltip anchor element.
    /// </summary>
    Dictionary<string, object?>? AnchorAttributes { get; set; }

    /// <summary>
    /// The main child content that serves as the anchor target for the tooltip.
    /// </summary>
    RenderFragment ChildContent { get; set; }

    /// <summary>
    /// Callback triggered when the tooltip becomes visible.
    /// </summary>
    EventCallback OnShow { get; set; }

    /// <summary>
    /// Callback triggered when the tooltip becomes hidden.
    /// </summary>
    EventCallback OnHide { get; set; }

    /// <summary>
    /// The full set of tooltip configuration options. Individual properties take precedence over this object.
    /// </summary>
    FloatingTooltipOptions Options { get; set; }

    /// <summary>
    /// Override: Tooltip placement relative to the anchor.
    /// </summary>
    FloatingTooltipPlacement? Placement { get; set; }

    /// <summary>
    /// Override: Whether the tooltip animates on show/hide.
    /// </summary>
    bool? Animate { get; set; }

    /// <summary>
    /// Override: Delay in milliseconds before the tooltip is shown.
    /// </summary>
    int? ShowDelay { get; set; }

    /// <summary>
    /// Override: Delay in milliseconds before the tooltip is hidden.
    /// </summary>
    int? HideDelay { get; set; }

    /// <summary>
    /// Override: Whether to show the tooltip arrow.
    /// </summary>
    bool? ShowArrow { get; set; }

    /// <summary>
    /// Override: Whether the tooltip is interactive (can receive pointer events).
    /// </summary>
    bool? Interactive { get; set; }

    /// <summary>
    /// Override: Whether the tooltip is enabled and active.
    /// </summary>
    bool? Enabled { get; set; }

    /// <summary>
    /// Override: Theme of the tooltip (e.g. dark, light).
    /// </summary>
    FloatingTooltipTheme? Theme { get; set; }

    /// <summary>
    /// Override: Maximum width of the tooltip in pixels.
    /// </summary>
    int? MaxWidth { get; set; }

    /// <summary>
    /// Override: If true, the tooltip must be manually triggered to show/hide.
    /// </summary>
    bool? ManualTrigger { get; set; }

    /// <summary>
    /// Override: Whether resources like scripts and styles should be loaded from CDN.
    /// </summary>
    bool? UseCdn { get; set; }

    /// <summary>
    /// Provides tooltip content as a RenderFragment (instead of plain text).
    /// </summary>
    /// <param name="content">The content to be rendered inside the tooltip.</param>
    void SetTooltipContent(RenderFragment content);

    /// <summary>
    /// Shows the tooltip manually. Only applicable if <see cref="ManualTrigger"/> is <c>true</c>.
    /// </summary>
    ValueTask Show();

    /// <summary>
    /// Hides the tooltip manually. Only applicable if <see cref="ManualTrigger"/> is <c>true</c>.
    /// </summary>
    ValueTask Hide();

    /// <summary>
    /// Toggles the tooltip visibility manually. Only applicable if <see cref="ManualTrigger"/> is <c>true</c>.
    /// </summary>
    ValueTask Toggle();
}