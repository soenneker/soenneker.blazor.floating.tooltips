using Microsoft.JSInterop;
using Soenneker.Blazor.Floating.Tooltips.Abstract;
using Soenneker.Blazor.Floating.Tooltips.Options;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;
using Soenneker.Utils.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Asyncs.Initializers;

namespace Soenneker.Blazor.Floating.Tooltips;

///<inheritdoc cref="IFloatingTooltipInterop"/>
public sealed class FloatingTooltipInterop : IFloatingTooltipInterop
{
    private const string _modulePath = "_content/Soenneker.Blazor.Floating.Tooltips/js/floatingtooltipinterop.js";

    private readonly IResourceLoader _resourceLoader;
    private readonly IModuleImportUtil _moduleImportUtil;
    private readonly AsyncInitializer<bool> _scriptInitializer;

    private readonly CancellationScope _cancellationScope = new();

    public FloatingTooltipInterop(IResourceLoader resourceLoader, IModuleImportUtil moduleImportUtil)
    {
        _resourceLoader = resourceLoader;
        _moduleImportUtil = moduleImportUtil;

        _scriptInitializer = new AsyncInitializer<bool>(InitializeScripts);
    }

    private async ValueTask InitializeScripts(bool useCdn, CancellationToken token)
    {
        if (useCdn)
        {
            await _resourceLoader.LoadScriptAndWaitForVariable(
                "https://cdn.jsdelivr.net/npm/@floating-ui/core@1.7.2/dist/floating-ui.core.umd.min.js",
                "FloatingUICore",
                "sha256-OhWDdFHrIg8eNZaNgWL2ax7tjKNFOBQq3WErqxfHdlQ=",
                cancellationToken: token);

            await _resourceLoader.LoadScriptAndWaitForVariable(
                "https://cdn.jsdelivr.net/npm/@floating-ui/dom@1.7.2/dist/floating-ui.dom.umd.min.js",
                "FloatingUIDOM",
                "sha256-cycZmidLw+l9uWDr4bUhL26YMJg1G6aM0AnUEPG9sME=",
                cancellationToken: token);
        }
        else
        {
            await _resourceLoader.LoadScriptAndWaitForVariable(
                NormalizeContentUri("_content/Soenneker.Blazor.Floating.Tooltips/js/floating-ui.core.umd.min.js"),
                "FloatingUIDOM",
                cancellationToken: token);

            await _resourceLoader.LoadScriptAndWaitForVariable(
                NormalizeContentUri("_content/Soenneker.Blazor.Floating.Tooltips/js/floating-ui.dom.umd.min.js"),
                "FloatingUIDOM",
                cancellationToken: token);
        }

        await _resourceLoader.LoadStyle(NormalizeContentUri("_content/Soenneker.Blazor.Floating.Tooltips/css/floatingtooltip.css"), cancellationToken: token);

        _ = await _moduleImportUtil.GetContentModuleReference(_modulePath, token);
    }

    private static string NormalizeContentUri(string uri)
    {
        if (string.IsNullOrEmpty(uri) || uri.Contains("://", StringComparison.Ordinal))
            return uri;

        return uri[0] == '/' ? uri : "/" + uri;
    }

    public async ValueTask Initialize(bool useCdn = true, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
            await _scriptInitializer.Init(useCdn, linked);
    }

    public async ValueTask Create(string id, FloatingTooltipOptions options, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await _scriptInitializer.Init(options.UseCdn, linked);

            string json = JsonUtil.Serialize(options)!;

            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("create", linked, id, json);
        }
    }

    public async ValueTask SetCallbacks(string id, DotNetObjectReference<FloatingTooltip> dotNetRef)
    {
        IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, CancellationToken.None);
        await module.InvokeVoidAsync("setCallbacks", CancellationToken.None, id, dotNetRef);
    }

    public async ValueTask Destroy(string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("dispose", linked, id);
        }
    }

    public async ValueTask Show(string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("show", linked, id);
        }
    }

    public async ValueTask Hide(string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("hide", linked, id);
        }
    }

    public async ValueTask Toggle(string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("toggle", linked, id);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _moduleImportUtil.DisposeContentModule(_modulePath);
        await _scriptInitializer.DisposeAsync();
        await _cancellationScope.DisposeAsync();
    }
}
