using Microsoft.Extensions.DependencyInjection;
using Soenneker.Blazor.Floating.Tooltips.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Registrars;

namespace Soenneker.Blazor.Floating.Tooltips.Registrars;

/// <summary>
/// A Blazor interop library using the modern library, floating-ui, for tooltips
/// </summary>
public static class FloatingTooltipRegistrar
{
    /// <summary>
    /// Adds <see cref="IFloatingTooltip"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddFloatingTooltipAsScoped(this IServiceCollection services)
    {
        services.AddResourceLoaderAsScoped().AddScoped<IFloatingTooltipInterop, FloatingTooltipInterop>();

        return services;
    }
}
