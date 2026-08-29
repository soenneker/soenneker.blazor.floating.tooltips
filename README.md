[![](https://img.shields.io/nuget/v/soenneker.blazor.floating.tooltips.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.floating.tooltips/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.floating.tooltips/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.floating.tooltips/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.floating.tooltips.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.floating.tooltips/)
[![](https://img.shields.io/badge/Demo-Live-blueviolet?style=for-the-badge&logo=github)](https://soenneker.github.io/soenneker.blazor.floating.tooltips/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.floating.tooltips/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.floating.tooltips/actions/workflows/codeql.yml)

# Soenneker.Blazor.Floating.Tooltips

A Blazor tooltip component positioned with [Floating UI](https://floating-ui.com/), with collision handling, delayed hover behavior, rich content, and manual control.

## Installation

```bash
dotnet add package Soenneker.Blazor.Floating.Tooltips
```

Register the scoped interop service:

```csharp
using Soenneker.Blazor.Floating.Tooltips.Registrars;

builder.Services.AddFloatingTooltipAsScoped();
```

Add these namespaces to `_Imports.razor`:

```razor
@using Soenneker.Blazor.Floating.Tooltips
@using Soenneker.Blazor.Floating.Tooltips.Enums
@using Soenneker.Blazor.Floating.Tooltips.Options
```

## Basic tooltip

```razor
<FloatingTooltip Text="Creates a copy"
                 Placement="FloatingTooltipPlacement.Top"
                 ShowDelay="300">
    <button type="button" aria-label="Copy invoice">Copy</button>
</FloatingTooltip>
```

The first child element is the positioning anchor. When the wrapper contains multiple elements, add `data-tooltip-anchor` to the element that should anchor the tooltip.

Tooltips should supplement an accessible label, not replace it: hover content is not reliably available to keyboard, touch, or assistive-technology users.

## Rich, interactive content

```razor
<FloatingTooltip Placement="FloatingTooltipPlacement.Bottom"
                 Interactive="true"
                 MaxWidth="320">
    <TooltipContent>
        <div>
            <strong>Keyboard shortcuts</strong>
            <div>Save: Ctrl+S</div>
        </div>
    </TooltipContent>

    <button type="button">Shortcuts</button>
</FloatingTooltip>
```

Use either `Text` or `TooltipContent`; setting both throws an `InvalidOperationException`. `Interactive` keeps the tooltip open while its content is hovered.

## Options

Inline parameters override values supplied through `Options`:

```razor
<FloatingTooltip Text="Saved"
                 Options="_defaults"
                 Theme="FloatingTooltipTheme.Success">
    <button type="button">Save</button>
</FloatingTooltip>

@code {
    private readonly FloatingTooltipOptions _defaults = new()
    {
        Placement = FloatingTooltipPlacement.Right,
        Animate = true,
        ShowArrow = true,
        ShowDelay = 150,
        HideDelay = 100,
        MaxWidth = 240,
        UseCdn = false
    };
}
```

Available placements are `Top`, `Bottom`, `Left`, and `Right`. Themes are `Dark`, `Light`, `Info`, `Success`, `Warning`, and `Error`. Configuration is applied when the tooltip is created; use `@key` to recreate the component when its configuration must change at runtime.

`UseCdn` defaults to `true`. Set it to `false` to load the packaged Floating UI scripts; the tooltip stylesheet is always loaded from the package.

## Manual control

```razor
<FloatingTooltip @ref="_tooltip"
                 Text="Copied"
                 ManualTrigger="true">
    <button type="button" @onclick="Copy">Copy</button>
</FloatingTooltip>

@code {
    private FloatingTooltip? _tooltip;

    private async Task Copy()
    {
        // Copy the value, then show confirmation.
        await _tooltip!.Show();
    }
}
```

`Show()`, `Hide()`, and `Toggle()` control a manual tooltip. `OnShow` and `OnHide` run when visibility actually changes, after any configured delay.
