using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Blazor.Floating.Tooltips.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Floating.Tooltips.Abstract;

/// <summary>
/// Provides JavaScript interop methods for initializing, controlling, and destroying Floating UI-based tooltips in a Blazor application.
/// </summary>
public interface IFloatingTooltipInterop
{
    /// <summary>
    /// Ensures all required JavaScript and CSS resources for Floating UI tooltips are loaded.
    /// This includes loading from CDN or embedded static files, depending on <paramref name="useCdn"/>.
    /// </summary>
    /// <param name="useCdn">Whether to load scripts from CDN or from local embedded resources.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    ValueTask Initialize(bool useCdn = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new tooltip instance using Floating UI logic, attaches it to a DOM anchor element, and registers it for automatic positioning.
    /// </summary>
    /// <param name="id">The unique ID used to resolve anchor and tooltip DOM elements (e.g., anchor-{id}, tooltip-{id}).</param>
    /// <param name="options">The configuration options used for positioning and middleware setup.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    ValueTask Create(string id, FloatingTooltipOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Registers a Blazor component's instance for receiving tooltip lifecycle event callbacks (e.g., OnShow, OnHide).
    /// </summary>
    /// <param name="id">The unique tooltip ID corresponding to the anchor element.</param>
    /// <param name="dotNetRef">A .NET reference to the component implementing the JSInvokable event handlers.</param>
    ValueTask SetCallbacks(string id, DotNetObjectReference<FloatingTooltip> dotNetRef);

    /// <summary>
    /// Destroys a previously initialized tooltip instance, unregistering its events and observers.
    /// </summary>
    /// <param name="id">The ID of the tooltip/anchor pair to clean up.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    ValueTask Destroy(string id, CancellationToken cancellationToken = default);

    ValueTask Show(string id, CancellationToken cancellationToken = default);

    ValueTask Hide(string id, CancellationToken cancellationToken = default);

    ValueTask Toggle(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disposes internal state and JavaScript module references.
    /// </summary>
    ValueTask DisposeAsync();
}
