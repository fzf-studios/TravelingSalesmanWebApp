using Microsoft.JSInterop;

namespace BlazorApp2.Domain.Helpers;
public interface IClipboardService
{
    public ValueTask<string> ReadTextAsync();
    public ValueTask WriteTextAsync(string text);
}

public sealed class ClipboardService: IClipboardService
{
    private readonly IJSRuntime _jsRuntime;

    public ClipboardService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public ValueTask<string> ReadTextAsync()
    {
        return _jsRuntime.InvokeAsync<string>("navigator.clipboard.readText");
    }

    public ValueTask WriteTextAsync(string text)
    {
        return _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
    }
}