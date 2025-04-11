[![](https://img.shields.io/nuget/v/soenneker.blazor.floating.tooltips.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.floating.tooltips/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.floating.tooltips/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.floating.tooltips/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.floating.tooltips.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.floating.tooltips/)

# ![Logo](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Blazor.Floating.Tooltips

### ⚡ A modern Blazor interop library for **Floating UI**-powered tooltips

> 🧪 **[Click here to try the demo](https://soenneker.github.io/soenneker.blazor.floating.tooltips/)**

`Soenneker.Blazor.Floating.Tooltips` is a fully featured Blazor component library that integrates with [Floating UI](https://floating-ui.com/) to provide sleek, customizable, and highly interactive tooltips using a pure C# API.

---

## ✨ Features

- ✅ Position-aware and collision-resistant tooltips
- 🎯 Custom placements, delays, and themes
- 🧲 Interactive and manually triggered tooltips
- 🎨 Dark/light theming with optional arrows
- 🔧 Full control via C# with event callbacks
- 🪶 Lightweight with optional CDN usage
- 🔁 Runtime toggle, show, and hide support
- 💥 Optimized for performance and resilience

---

## 📦 Installation

```bash
dotnet add package Soenneker.Blazor.Floating.Tooltips
```

Register:

```csharp
services.AddFloatingTooltipAsScoped();
```

---

## 🛠️ Usage

Basic example with plain string content:

```razor
<FloatingTooltip ContentText="Hello tooltip!">
    <button class="btn">Hover me</button>
</FloatingTooltip>
```

With full options and event handling:

```razor
<FloatingTooltip
                 ContentText="This is an interactive tooltip"
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
    private void HandleShow() => Console.WriteLine("Tooltip shown!");
    private void HandleHide() => Console.WriteLine("Tooltip hidden!");
}
```

Using `TooltipContent` instead of `ContentText`:

```razor
<FloatingTooltip>
    <TooltipContent>
        <div class="p-2">🧠 <strong>Smart Tooltip</strong><br />Rich content goes here</div>
    </TooltipContent>
    <button class="btn btn-primary">Hover me</button>
</FloatingTooltip>
```

---

## ⚙️ Options

You can use the `Options` parameter or inline params:

```razor
Options = new FloatingTooltipOptions
{
    Animate = true,
    ShowArrow = true,
    Theme = FloatingTooltipTheme.Light,
    MaxWidth = 250,
    ManualTrigger = false,
    UseCdn = true
}
```

Or shorthand:

```razor
Animate="true" Theme="FloatingTooltipTheme.Dark"
```

---

## 🧩 Interop Methods

You can control tooltips programmatically via:

```csharp
await tooltipRef.Show();
await tooltipRef.Hide();
await tooltipRef.Toggle();
```