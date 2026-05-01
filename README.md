[![](https://img.shields.io/nuget/v/soenneker.blazor.floating.tooltips.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.floating.tooltips/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.floating.tooltips/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.floating.tooltips/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.floating.tooltips.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.floating.tooltips/)
[![](https://img.shields.io/badge/Demo-Live-blueviolet?style=for-the-badge&logo=github)](https://soenneker.github.io/soenneker.blazor.floating.tooltips/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.floating.tooltips/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.floating.tooltips/actions/workflows/codeql.yml)

# <img src="https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png" alt="Logo" width="48"/> Soenneker.Blazor.Floating.Tooltips

> Modern Blazor tooltips powered by Floating UI.

`Soenneker.Blazor.Floating.Tooltips` is a Blazor component library that integrates with [Floating UI](https://floating-ui.com/) to provide position-aware, customizable, and interactive tooltips with a C# API.

[Open the demo site](https://soenneker.github.io/soenneker.blazor.floating.tooltips/)

---

## Features

- Position-aware and collision-resistant tooltip placement
- Custom placements, show/hide delays, themes, and arrows
- Interactive tooltip content
- Manual show, hide, and toggle support from C#
- Event callbacks for show and hide
- Optional CDN loading for Floating UI scripts
- Packaged static web assets for local script and stylesheet loading

---

## Installation

```bash
dotnet add package Soenneker.Blazor.Floating.Tooltips
```

Register the service:

```csharp
using Soenneker.Blazor.Floating.Tooltips.Registrars;

builder.Services.AddFloatingTooltipAsScoped();
```

Add the namespace where you use the components:

```razor
@using Soenneker.Blazor.Floating.Tooltips
@using Soenneker.Blazor.Floating.Tooltips.Enums
@using Soenneker.Blazor.Floating.Tooltips.Options
```

---

## Usage

Basic tooltip with plain text:

```razor
<FloatingTooltip Text="Hello tooltip!">
    <button class="btn">Hover me</button>
</FloatingTooltip>
```

Tooltip with options and event callbacks:

```razor
<FloatingTooltip Text="This is an interactive tooltip"
                 Placement="FloatingTooltipPlacement.Top"
                 Animate="true"
                 ShowArrow="true"
                 Interactive="true"
                 OnShow="HandleShow"
                 OnHide="HandleHide"
                 Theme="FloatingTooltipTheme.Dark">
    <span class="text-muted">Hover over me</span>
</FloatingTooltip>

@code {
    private void HandleShow() => Console.WriteLine("Tooltip shown");
    private void HandleHide() => Console.WriteLine("Tooltip hidden");
}
```

Rich content with `TooltipContent`:

```razor
<FloatingTooltip>
    <TooltipContent>
        <div class="p-2">
            <strong>Smart Tooltip</strong><br />
            Rich content goes here.
        </div>
    </TooltipContent>

    <button class="btn btn-primary">Hover me</button>
</FloatingTooltip>
```

---

## Options

You can set options through `FloatingTooltipOptions`:

```razor
@using Soenneker.Blazor.Floating.Tooltips.Enums
@using Soenneker.Blazor.Floating.Tooltips.Options

<FloatingTooltip Text="Configured tooltip" Options="_options">
    <button>Hover me</button>
</FloatingTooltip>

@code {
    private readonly FloatingTooltipOptions _options = new()
    {
        Animate = true,
        ShowArrow = true,
        Theme = FloatingTooltipTheme.Light,
        MaxWidth = 250,
        ManualTrigger = false,
        UseCdn = true
    };
}
```

Or set the same values inline:

```razor
<FloatingTooltip Text="Inline options"
                 Animate="true"
                 Theme="FloatingTooltipTheme.Dark"
                 ShowArrow="true">
    <button>Hover me</button>
</FloatingTooltip>
```

`UseCdn` defaults to `true`. When set to `false`, the library loads its bundled static web assets from `_content/Soenneker.Blazor.Floating.Tooltips/...`, which works with Blazor base paths such as GitHub Pages project sites.

---

## Programmatic Control

Use `@ref` to control a tooltip manually:

```razor
<FloatingTooltip @ref="_tooltip"
                 Text="Controlled manually"
                 ManualTrigger="true">
    <button @onclick="ToggleTooltip">Toggle tooltip</button>
</FloatingTooltip>

@code {
    private FloatingTooltip? _tooltip;

    private async Task ToggleTooltip()
    {
        if (_tooltip is not null)
            await _tooltip.Toggle();
    }
}
```

Available methods:

```csharp
await tooltip.Show();
await tooltip.Hide();
await tooltip.Toggle();
```
