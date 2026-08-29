using Microsoft.Extensions.DependencyInjection;
using Soenneker.Blazor.Floating.Tooltips.Abstract;
using Soenneker.Blazor.Interops.Floating.Registrars;

namespace Soenneker.Blazor.Floating.Tooltips.Registrars;

/// <summary>
/// A Blazor interop library using the modern library, floating-ui, for tooltips
/// </summary>
public static class FloatingTooltipRegistrar
{
    /// <summary>
    /// Adds <see cref="IFloatingTooltip"/> as a scoped service. <para/>
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddFloatingTooltipAsScoped(this IServiceCollection services)
    {
        services.AddFloatingUiInteropAsScoped()
                .AddScoped<IFloatingTooltipInterop, FloatingTooltipInterop>();

        return services;
    }
}
