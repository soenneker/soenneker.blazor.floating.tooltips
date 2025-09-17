using Microsoft.JSInterop;
using Soenneker.Blazor.Floating.Tooltips.Abstract;
using Soenneker.Blazor.Floating.Tooltips.Options;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Utils.AsyncSingleton;
using Soenneker.Utils.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Floating.Tooltips;

///<inheritdoc cref="IFloatingTooltipInterop"/>
public sealed class FloatingTooltipInterop : IFloatingTooltipInterop
{
    private readonly IResourceLoader _resourceLoader;
    private readonly AsyncSingleton _scriptInitializer;
    private readonly IJSRuntime _jSRuntime;

    private const string _module = "Soenneker.Blazor.Floating.Tooltips/js/floatingtooltipinterop.js";
    private const string _moduleName = "FloatingTooltipInterop";

    public FloatingTooltipInterop(IJSRuntime jSRuntime, IResourceLoader resourceLoader)
    {
        _jSRuntime = jSRuntime;
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncSingleton(async (token, arg) =>
        {
            var useCdn = (bool) arg[0];

            if (useCdn)
            {
                await _resourceLoader.LoadScriptAndWaitForVariable("https://cdn.jsdelivr.net/npm/@floating-ui/core@1.7.2/dist/floating-ui.core.umd.min.js",
                                         "FloatingUICore", "sha256-OhWDdFHrIg8eNZaNgWL2ax7tjKNFOBQq3WErqxfHdlQ=", cancellationToken: token)
                                     ;

                await _resourceLoader.LoadScriptAndWaitForVariable("https://cdn.jsdelivr.net/npm/@floating-ui/dom@1.7.2/dist/floating-ui.dom.umd.min.js",
                                         "FloatingUIDOM", "sha256-cycZmidLw+l9uWDr4bUhL26YMJg1G6aM0AnUEPG9sME=", cancellationToken: token)
                                     ;
            }
            else
            {
                await _resourceLoader.LoadScriptAndWaitForVariable("_content/Soenneker.Blazor.Floating.Tooltips/js/floating-ui.core.umd.min.js",
                                         "FloatingUIDOM", cancellationToken: token)
                                     ;

                await _resourceLoader.LoadScriptAndWaitForVariable("_content/Soenneker.Blazor.Floating.Tooltips/js/floating-ui.dom.umd.min.js", "FloatingUIDOM",
                                         cancellationToken: token)
                                     ;
            }

            await _resourceLoader.LoadStyle("_content/Soenneker.Blazor.Floating.Tooltips/css/floatingtooltip.css", cancellationToken: token);
            await _resourceLoader.ImportModuleAndWaitUntilAvailable(_module, _moduleName, 100, token);

            return new object();
        });
    }

    public ValueTask Initialize(bool useCdn = true, CancellationToken cancellationToken = default)
    {
        return _scriptInitializer.Init(cancellationToken, useCdn);
    }

    public async ValueTask Create(string id, FloatingTooltipOptions options, CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Init(cancellationToken, options.UseCdn);

        string json = JsonUtil.Serialize(options)!;

        await _jSRuntime.InvokeVoidAsync($"{_moduleName}.create", cancellationToken, id, json);
    }

    public ValueTask SetCallbacks(string id, DotNetObjectReference<FloatingTooltip> dotNetRef)
    {
        return _jSRuntime.InvokeVoidAsync($"{_moduleName}.setCallbacks", id, dotNetRef);
    }

    public ValueTask Destroy(string id, CancellationToken cancellationToken = default)
    {
        return _jSRuntime.InvokeVoidAsync($"{_moduleName}.dispose", cancellationToken, id);
    }

    public ValueTask Show(string id, CancellationToken cancellationToken = default)
    {
        return _jSRuntime.InvokeVoidAsync($"{_moduleName}.show", cancellationToken, id);
    }

    public ValueTask Hide(string id, CancellationToken cancellationToken = default)
    {
        return _jSRuntime.InvokeVoidAsync($"{_moduleName}.hide", cancellationToken, id);
    }

    public ValueTask Toggle(string id, CancellationToken cancellationToken = default)
    {
        return _jSRuntime.InvokeVoidAsync($"{_moduleName}.toggle", cancellationToken, id);
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_module);
        await _scriptInitializer.DisposeAsync();
    }
}
